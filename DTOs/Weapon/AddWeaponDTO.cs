namespace jumpstart_ud.DTOs.Weapon
{
    public class AddWeaponDTO
    {
        public string Name { get; set; } = String.Empty;
        public int Damage { get; set; }
        public int CharacterId { get; set; }
    }
}
