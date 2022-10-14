using jumpstart_ud.DTOs.Character;
using jumpstart_ud.Models;

namespace jumpstart_ud.Services.CharacterService
{
    public interface ICharacterService
    {
        /// <summary>
        /// ICharacterService
        /// </summary>
        /// <returns>List of GetCharacterDTO</returns>
        Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters();

        Task<ServiceResponse<GetCharacterDTO>> GetCharacterById(int id);

        Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO newCharacter);

        Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updatedCharacter);

        Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id);
    }
}
