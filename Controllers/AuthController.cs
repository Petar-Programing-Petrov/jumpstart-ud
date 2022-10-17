using jumpstart_ud.Data;
using jumpstart_ud.DTOs.User;
using jumpstart_ud.Models;
using Microsoft.AspNetCore.Mvc;

namespace jumpstart_ud.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }





        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDTO request)
        {
            var responce = await _authRepository.Register(

                new User { Username = request.Username }, request.Password
            );

            if (!responce.Success)
            {
                return BadRequest(responce);
            }

            return Ok(responce);
        }



        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDTO request)
        {
            var responce = await _authRepository.Login(request.Username, request.Password);

            if (!responce.Success)
            {
                return BadRequest(responce);
            }

            return Ok(responce);
        }
    }
}
