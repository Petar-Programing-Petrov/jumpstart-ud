using jumpstart_ud.DTOs.Character;
using jumpstart_ud.DTOs.Weapon;
using jumpstart_ud.Models;

namespace jumpstart_ud.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDTO>> AddWeapon(AddWeaponDTO newWeapon);
    }
}
