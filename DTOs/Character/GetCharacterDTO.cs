
using jumpstart_ud.DTOs.Skill;
using jumpstart_ud.DTOs.Weapon;
using jumpstart_ud.Models;

namespace jumpstart_ud.DTOs.Character
{
    public class GetCharacterDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = "Frodo";

        public int HitPoints { get; set; } = 100;

        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 10;
        public int Intelligence { get; set; } = 10;

        public RpgClass Class { get; set; } = RpgClass.Knight;

        public GetWeaponDTO Weapon { get; set; }

        public List<GetSkillDTO> Skills { get; set; }
    }
}
