using jumpstart_ud.Models;

namespace jumpstart_ud.DTOs.Character
{
    public class UpdateCharacterDTO
    {

        public int Id { get; set; }

        public string Name { get; set; } = "Frodo";

        public int HitPoints { get; set; } = 100;

        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 10;
        public int Intelligence { get; set; } = 10;

        public RpgClass Class { get; set; } = RpgClass.Knight;


        //public string Name { get; set; } 

        //public int HitPoints { get; set; }

        //public int Strength { get; set; } 
        //public int Defense { get; set; } 
        //public int Intelligence { get; set; }

        //public RpgClass Class { get; set; } = new RpgClass();
    }
}
