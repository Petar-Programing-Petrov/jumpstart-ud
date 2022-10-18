using AutoMapper;
using jumpstart_ud.Data;
using jumpstart_ud.DTOs.Character;
using jumpstart_ud.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace jumpstart_ud.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
       
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
           _mapper = mapper;
           _context = context;
           _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// method to get the user id from httpContext.User(ClaimTypes.NameIdentifier)
        /// </summary>
        /// <returns></returns>
        private int GetUSerId() => int.Parse(_httpContextAccessor.HttpContext.User
            .FindFirstValue(ClaimTypes.NameIdentifier));

        /// <summary>
        /// Add character
        /// </summary>
        /// <param name="newCharacter"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            Character character = _mapper.Map<Character>(newCharacter);

            //Add character for logged user
            character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUSerId());

            //Add the character to DataContext
            _context.Characters.Add(character);

            //Write the data in the DB
            await _context.SaveChangesAsync();

            //Return all characters from the DB for the Logged user by using .Where and passing the GetUserId Method
            serviceResponse.Data = await _context.Characters
                .Where(c => c.User.Id == GetUSerId())
                .Select(c => _mapper.Map<GetCharacterDTO>(c))
                .ToListAsync();

            return serviceResponse;
        }

        /// <summary>
        /// Get a character by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<GetCharacterDTO>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDTO>();
            var dbCharacter = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUSerId());
            serviceResponse.Data = _mapper.Map<GetCharacterDTO>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters()
        {
            var response = new ServiceResponse<List<GetCharacterDTO>>();
            var dbCharacters = await _context.Characters
                .Where(c => c.User.Id == GetUSerId())
                .ToListAsync();
            response.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();

            return response;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updatedCharacter)
        {
            ServiceResponse<GetCharacterDTO> response = new ServiceResponse<GetCharacterDTO>();

            try
            {
                //Search for the character in DB per given ID and include the User
                var character = await _context.Characters
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);

                //_mapper.Map<Character>(updatedCharacter);                
                //_mapper.Map(updatedCharacter, character);

                
                //Check if the character.User.Id is same as the logged user

                if (character.User.Id == GetUSerId())
                {
                    //Overwrite the prop of the character
                    character.Name = updatedCharacter.Name;
                    character.HitPoints = updatedCharacter.HitPoints;
                    character.Strength = updatedCharacter.Strength;
                    character.Defense = updatedCharacter.Defense;
                    character.Intelligence = updatedCharacter.Intelligence;
                    character.Class = updatedCharacter.Class;

                    //Save the new changes to the Database
                    await _context.SaveChangesAsync();

                    //Return the character with the GetCharacterDTO 
                    response.Data = _mapper.Map<GetCharacterDTO>(character);

                }
                else
                {
                    response.Success = false;
                    response.Message = "Character not found!";
                }
                
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message ;
            }

            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDTO>> response = new ServiceResponse<List<GetCharacterDTO>>();

            try
            {
                Character character = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUSerId());

                if (character !=  null)
                {
                    _context.Characters.Remove(character);
                    await _context.SaveChangesAsync();
                    response.Data = _context.Characters
                        .Where(c => c.User.Id == GetUSerId())
                        .Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();
                }
                else
                {
                    response.Success = false;
                    response.Message = "Character not found!";
                }


            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            //small git test

            return response;
        }
    
    }
}
