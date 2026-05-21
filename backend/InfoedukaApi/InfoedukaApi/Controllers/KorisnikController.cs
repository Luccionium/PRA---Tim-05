using InfoedukaApi.Models;
using InfoedukaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfoedukaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class KorisnikController : ControllerBase
    {
        private readonly KorisnikService _korisnikService;

        public KorisnikController(KorisnikService korisnikService)
        {
            _korisnikService = korisnikService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var korisnici = await _korisnikService.GetAll();
            return Ok(korisnici);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var korisnik = await _korisnikService.GetById(id);
            if (korisnik == null)
                return NotFound("Korisnik nije pronađen.");

            return Ok(korisnik);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Korisnik korisnik)
        {
            var novi = await _korisnikService.Create(korisnik);
            return CreatedAtAction(nameof(GetById), new { id = novi.Id }, novi);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, Korisnik korisnik)
        {
            var success = await _korisnikService.Update(id, korisnik);
            if (!success)
                return NotFound("Korisnik nije pronađen.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _korisnikService.Delete(id);
            if (!success)
                return NotFound("Korisnik nije pronađen.");

            return NoContent();
        }
    }
}