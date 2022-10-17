using AutoMapper;
using jumpstart_ud.Data;
using jumpstart_ud.DTOs.Character;
using jumpstart_ud.Models;
using Microsoft.EntityFrameworkCore;

namespace jumpstart_ud.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
       
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
           _mapper = mapper;
           _context = context;
        }
        public async Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            Character character = _mapper.Map<Character>(newCharacter);

            //Add the character to DataContext
            _context.Characters.Add(character);

            //Write the data in the DB
            await _context.SaveChangesAsync();

            //Return all characters from the 
            serviceResponse.Data = await _context.Characters
                .Select(c => _mapper.Map<GetCharacterDTO>(c))
                .ToListAsync();

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDTO>();
            var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
            serviceResponse.Data = _mapper.Map<GetCharacterDTO>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters()
        {
            var response = new ServiceResponse<List<GetCharacterDTO>>();
            var dbCharacters = await _context.Characters.ToListAsync();
            response.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();

            return response;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updatedCharacter)
        {
            ServiceResponse<GetCharacterDTO> response = new ServiceResponse<GetCharacterDTO>();

            try
            {
                //Search for the character in DB per given ID

                var character = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);

                                         //_mapper.Map<Character>(updatedCharacter);                
                                        //_mapper.Map(updatedCharacter, character);

                //Overwriting the prop of the character

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
                Character character = await _context.Characters.FirstAsync(c => c.Id == id);
                
                _context.Characters.Remove(character);


                await _context.SaveChangesAsync();

                response.Data =  _context.Characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();

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
