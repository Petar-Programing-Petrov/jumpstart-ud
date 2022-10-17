
using jumpstart_ud.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace jumpstart_ud.Data
{

    //Dependency Injection 
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;

        public object SymetricSecutiryKey { get; private set; }

        public AuthRepository(DataContext context , IConfiguration config)
        {
            _context = context;
            _config = config;
        }


       



        /// <summary>
        /// Registration method
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns>ServiceResponse response.Success and response.Message if an error ocures</returns>
        public async Task<ServiceResponse<int>> Register(User user, string password)
        {

            ServiceResponse<int> response = new ServiceResponse<int>();

            if (await UserExists(user.Username))
            {
                response.Success = false;
                response.Message = "User already exists";
                return response;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            response.Data = user.Id;
            return response;
        }



        /// <summary>
        /// Login method
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var response = new ServiceResponse<string>();

            //Find the user in Db
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username.ToLower().Equals(username.ToLower()));

            if (user == null)
            {
                //If the user is not the same as in Database
                response.Success = false;
                response.Message = "User not found";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                //If the pasword is wrong
                response.Success = false;
                response.Message = "Wrong password!";
            }
            else
            {
                //Create a Token for the user
                response.Data = CreateToken(user);
            }


            return response;
        }



        /// <summary>
        /// Takes a string and generates a password hash and a password salt
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }



        /// <summary>
        /// Create a token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);


            return tokenHandler.WriteToken(token);
        }



        //Password verification
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        //Check if User exsits
        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }
    }
}
