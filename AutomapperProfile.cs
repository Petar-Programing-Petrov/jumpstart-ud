using AutoMapper;
using jumpstart_ud.DTOs.Character;
using jumpstart_ud.DTOs.Weapon;
using jumpstart_ud.Models;

namespace jumpstart_ud
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Character, GetCharacterDTO>();
            CreateMap<AddCharacterDTO, Character>();
            //To use automapper in UpdateCharacter uncomment the mapping below
            //CreateMap<UpdateCharacterDTO, Character>();
            CreateMap<Weapon, GetWeaponDTO>();
            
        }
    }
}
