using jumpstart_ud.DTOs.Character;
using jumpstart_ud.DTOs.Weapon;
using jumpstart_ud.Models;
using jumpstart_ud.Services.WeaponService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace jumpstart_ud.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeaponController :ControllerBase
    {
        private readonly IWeaponService _weaponService;

        public WeaponController(IWeaponService weaponService)
        {
            _weaponService = weaponService;
        }



        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> AddWeapon(AddWeaponDTO newWeapon)
        {
            return Ok(await _weaponService.AddWeapon(newWeapon));
        }
    }
}
