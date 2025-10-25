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
    }
}
