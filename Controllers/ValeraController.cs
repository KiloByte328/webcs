using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using web.Service;
using web.Models;
using web.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace web.ValeraController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
    private readonly IJwtService _authService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AuthController(
        UserManager<IdentityUser> userManager, 
        SignInManager<IdentityUser> signInManager,
        IJwtService authService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authService = authService;
    }
        [HttpPost("register")]
        public async Task<IActionResult> RegUser([FromBody] RegisterDTO model)
        {
            var user = new IdentityUser { UserName = model.Username, Email = model.Email};
            var res = await _userManager.CreateAsync(user, model.Password);
            if (!res.Succeeded)
            {
                return BadRequest(new { Errors = res.Errors.Select(e => e.Description) });
            }
            await _userManager.AddToRoleAsync(user, "User");
            if (user.UserName == "Admin")
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            var token = await _authService.GenerateToken(user);
            return Ok(new {Token = token, });
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized( new { Message = "такого пользователя не существует"});
            }
            var res = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!res)
            {
                return Unauthorized( new { Message = "неверный пароль"});
            }
            var token = await _authService.GenerateToken(user);
            return Ok(new {Token = token, });
        }
    }
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ValeraController : ControllerBase
    {
        public readonly ValeraService _valeraService;
        public ValeraController(ValeraService valeraService)
        {
            _valeraService = valeraService;
        }
        [HttpGet("AllValeras")]
        public async Task<IActionResult> GetValeras()
        {
            var curUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Valera> valeras;
            if (User.IsInRole("Admin"))
            {
               valeras = await _valeraService.GetAllValeras();
            }
            else
            {
                valeras = await _valeraService.GetValerasByOwner(curUser!);
            }
            return Ok(valeras);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetValeraById(int id)
        {
            var valeras = await _valeraService.GetValeraById(id);
            if (valeras == null)
            {
                return NotFound();
            }
            return Ok(valeras);

        }
        [HttpPost]
        public async Task<IActionResult> CreateValera([FromBody] ValeraDTO valeraDto)
        {
            var curUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Valera? valera = null;
            if (valeraDto != null)
            {
                valera = new Valera
                {
                    Name = valeraDto.Name ?? "Valera",
                    HP = valeraDto.HP ?? 100,
                    MP = valeraDto.MP ?? 0,
                    FT = valeraDto.FT ?? 0,
                    CF = valeraDto.CF ?? 0,
                    MN = valeraDto.MN ?? 0,
                    ValeraOwner = curUser!
                };
            }
            if (valera == null)
                valera = new Valera();
            await _valeraService.AddValeraToDb(valera);
            return CreatedAtAction(nameof(GetValeraById), new { id = valera.Id }, valera);
        }
        [HttpGet]
        public async Task<IActionResult> GetValeras([FromQuery] string? search)
        {
            var curUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var valeras = await _valeraService.GetAllValeras();
        
            if (!string.IsNullOrEmpty(search))
                valeras = valeras.Where(v => v.Name.Contains(search, StringComparison.OrdinalIgnoreCase) && v.ValeraOwner == curUser!).ToList();
        
            return Ok(valeras);
        }
        [HttpPost("{id}/work")]
        public async Task<IActionResult> ValeraGoToWork(int id)
        {
            var valera = await _valeraService.GetValeraById(id);
            if (valera == null)
            {
                return NotFound();
            }
            valera.go_to_work();
            await _valeraService.UpdateValeraInDb(valera);
            return Ok(valera);
        }
        [HttpPost("{id}/touch_grass")]
        public async Task<IActionResult> ValeraGoToTouchGrass(int id)
        {
            var valera = await _valeraService.GetValeraById(id);
            if (valera == null)
            {
                return NotFound();
            }
            valera.go_to_touch_grass();
            await _valeraService.UpdateValeraInDb(valera);
            return Ok(valera);
        }
        [HttpPost("{id}/cinema")]
        public async Task<IActionResult> ValeraGoToCinema(int id)
        {
            var valera = await _valeraService.GetValeraById(id);
            if (valera == null)
            {
                return NotFound();
            }
            valera.go_to_cinema();
            await _valeraService.UpdateValeraInDb(valera);
            return Ok(valera);
        }
        [HttpPost("{id}/go_to_pub")]
        public async Task<IActionResult> ValeraGoToPub(int id)
        {
            var valera = await _valeraService.GetValeraById(id);
            if (valera == null)
            {
                return NotFound();
            }
            valera.go_to_pub();
            await _valeraService.UpdateValeraInDb(valera);
            return Ok(valera);
        }
        [HttpPost("{id}/sleep")]
        public async Task<IActionResult> ValeraGoToSleep(int id)
        {
            var valera = await _valeraService.GetValeraById(id);
            if (valera == null)
            {
                return NotFound();
            }
            valera.go_to_sleep();
            await _valeraService.UpdateValeraInDb(valera);
            return Ok(valera);
        }
        [HttpPost("{id}/sing_in_metro")]
        public async Task<IActionResult> ValeraGoSingInMetro(int id)
        {
            var valera = await _valeraService.GetValeraById(id);
            if (valera == null)
            {
                return NotFound();
            }
            valera.go_sing_in_metro();
            await _valeraService.UpdateValeraInDb(valera);
            return Ok(valera);
        }
        [HttpPost("{id}/go_to_drink_with")]
        public async Task<IActionResult> ValeraGoToDrinkWith(int id)
        {
            var valera = await _valeraService.GetValeraById(id);
            if (valera == null)
            {
                return NotFound();
            }
            valera.go_to_drink_with();
            await _valeraService.UpdateValeraInDb(valera);
            return Ok(valera);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteValera(int id)
        {
            var valera = await _valeraService.GetValeraById(id);
            if (valera == null)
            {
                return NotFound();
            }
            await _valeraService.DeleteValera(valera);
            return NoContent();
        }
    }
}
