using InfoedukaApi.Models;
using InfoedukaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InfoedukaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class KolegijController : ControllerBase
    {
        private readonly KolegijService _kolegijService;

        public KolegijController(KolegijService kolegijService)
        {
            _kolegijService = kolegijService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tip = User.FindFirst(ClaimTypes.Role)?.Value;
            var idClaim = User.FindFirst("id")?.Value;

            if (tip == "Predavac" && idClaim != null)
            {
                var predavacId = int.Parse(idClaim);
                var kolegiji = await _kolegijService.GetByPredavac(predavacId);
                return Ok(kolegiji);
            }

            var svi = await _kolegijService.GetAll();
            return Ok(svi);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var kolegij = await _kolegijService.GetById(id);
            if (kolegij == null)
                return NotFound("Kolegij nije pronađen.");

            return Ok(kolegij);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Kolegij kolegij)
        {
            var novi = await _kolegijService.Create(kolegij);
            return CreatedAtAction(nameof(GetById), new { id = novi.Id }, novi);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, Kolegij kolegij)
        {
            var success = await _kolegijService.Update(id, kolegij);
            if (!success)
                return NotFound("Kolegij nije pronađen.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _kolegijService.Delete(id);
            if (!success)
                return NotFound("Kolegij nije pronađen.");

            return NoContent();
        }

        [HttpPost("{kolegijId}/predavac/{predavacId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPredavac(int kolegijId, int predavacId)
        {
            var success = await _kolegijService.AddPredavac(kolegijId, predavacId);
            if (!success)
                return NotFound("Kolegij ili predavač nisu pronađeni.");

            return NoContent();
        }

        [HttpDelete("{kolegijId}/predavac/{predavacId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemovePredavac(int kolegijId, int predavacId)
        {
            var success = await _kolegijService.RemovePredavac(kolegijId, predavacId);
            if (!success)
                return NotFound("Kolegij ili predavač nisu pronađeni.");

            return NoContent();
        }
    }
}