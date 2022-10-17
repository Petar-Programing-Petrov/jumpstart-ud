using jumpstart_ud.DTOs.Character;
using jumpstart_ud.Models;
using jumpstart_ud.Services.CharacterService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace jumpstart_ud.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;
        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }
        /// <summary>
        /// Get all characters
        /// </summary>
        /// <returns>List of characters</returns>        
        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> GetAll()
        {
            return Ok(await _characterService.GetAllCharacters());
        }
        /// <summary>
        /// Get the character by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Character by his id</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> GetSingle(int id)
        {
            return Ok(await _characterService.GetCharacterById(id));
        }
        /// <summary>
        /// Delete character by his id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> Delete(int id)
        {
            var response = await _characterService.DeleteCharacter(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> AddCharacter(AddCharacterDTO newCharacter)
        {
            return Ok(await _characterService.AddCharacter(newCharacter));
        }
        /// <summary>
        /// Edit character
        /// </summary>
        /// <param name="updateCharacter"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> AddCharacter(UpdateCharacterDTO updateCharacter)
        {
            var response = await _characterService.UpdateCharacter(updateCharacter);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(await _characterService.UpdateCharacter(updateCharacter));
        }


    }
} 