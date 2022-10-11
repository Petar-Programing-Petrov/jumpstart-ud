using AutoMapper;
using jumpstart_ud.DTOs.Character;
using jumpstart_ud.Models;

namespace jumpstart_ud
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Character, GetCharacterDTO>();
            CreateMap<AddCharacterDTO, Character>();
        }
    }
}
