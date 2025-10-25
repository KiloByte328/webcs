using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web.Service;
using web.Models;

namespace web.ValeraController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValeraController : ControllerBase
    {
        public readonly ValeraService _valeraService;
        public ValeraController(ValeraService valeraService)
        {
            _valeraService = valeraService;
        }
        [HttpGet]
        public IActionResult GetValeras()
        {
            var valeras = _valeraService.GetAllValeras();
            return Ok(valeras);
        }
        [HttpGet]
        public IActionResult GetValeraStatus()
        {
            var valeras = _valeraService.GetAllValeras();
            return Ok(valeras);
        }
        [HttpGet("{id}")]
        public IActionResult GetValeraById(int id)
        {
            var valeras = _valeraService.GetAllValeras().FirstOrDefault(v => v.Id == id);
            if (valeras == null)
            {
                return NotFound();
            }
            return Ok(valeras);

        }
        [HttpPost]
        public IActionResult CreateValera(Valera valera)
        {
            _valeraService.AddValeraToDb(valera);
            return CreatedAtAction(nameof(GetValeraById), new { id = valera.Id }, valera);
        }
        [HttpPost("{id}/work")]
        public IActionResult ValeraGoToWork(int id)
        {
            var valera = _valeraService.GetValeraById(id);
            if (valera == null)
            {
                return NotFound();
            }
            valera.go_to_work();
            _valeraService.UpdateValeraInDb(valera);
            return Ok(valera);
        }
        [HttpPost("{id}/touch_grass")]
        public IActionResult ValeraGoToTouchGrass(int id)
        {
            var valera = _valeraService.GetValeraById(id);
            if (valera == null)
            {
                return NotFound();
            }
            valera.go_to_touch_grass();
            _valeraService.UpdateValeraInDb(valera);
            return Ok(valera);
        }
        [HttpPost("{id}/cinema")]
        public IActionResult ValeraGoToCinema(int id)
        {
            var valera = _valeraService.GetValeraById(id);
            if (valera == null)
            {
                return NotFound();
            }
            valera.go_to_cinema();
            _valeraService.UpdateValeraInDb(valera);
            return Ok(valera);
        }
        [HttpPost("{id}/go_to_pub")]
        public IActionResult ValeraGoToPub(int id)
        {
            var valera = _valeraService.GetValeraById(id);
            if (valera == null)
            {
                return NotFound();
            }
            valera.go_to_pub();
            _valeraService.UpdateValeraInDb(valera);
            return Ok(valera);

        }
        [HttpPost("{id}/sleep")]
        public IActionResult ValeraGoToSleep(int id)
        {
            var valera = _valeraService.GetValeraById(id);
            if (valera == null)
            {
                return NotFound();
            }
            valera.go_to_sleep();
            _valeraService.UpdateValeraInDb(valera);
            return Ok(valera);
        }
        [HttpPost("{id}/sing_in_metro")]
        public IActionResult ValeraGoSingInMetro(int id)
        {
            var valera = _valeraService.GetValeraById(id);
            if (valera == null)
            {
                return NotFound();
            }
            valera.go_sing_in_metro();
            _valeraService.UpdateValeraInDb(valera);
            return Ok(valera);
        }
        [HttpPost("{id}/go_to_drink_with")]
        public IActionResult ValeraGoToDrinkWith(int id)
        {
            var valera = _valeraService.GetValeraById(id);
            if (valera == null)
            {
                return NotFound();
            }
            valera.go_to_drink_with();
            _valeraService.UpdateValeraInDb(valera);
            return Ok(valera);
        }
    }
}
