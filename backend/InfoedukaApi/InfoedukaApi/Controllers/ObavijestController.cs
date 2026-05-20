using InfoedukaApi.Models;
using InfoedukaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfoedukaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ObavijestController : ControllerBase
    {
        private readonly ObavijestService _obavijestService;
        private readonly KolegijService _kolegijService;

        public ObavijestController(ObavijestService obavijestService, KolegijService kolegijService)
        {
            _obavijestService = obavijestService;
            _kolegijService = kolegijService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var obavijesti = await _obavijestService.GetAll();
            return Ok(obavijesti);
        }

        [HttpGet("kolegij/{kolegijId}")]
        public async Task<IActionResult> GetByKolegij(int kolegijId)
        {
            var obavijesti = await _obavijestService.GetByKolegij(kolegijId);
            return Ok(obavijesti);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var obavijest = await _obavijestService.GetById(id);
            if (obavijest == null)
                return NotFound("Obavijest nije pronađena.");

            return Ok(obavijest);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Obavijest obavijest)
        {
            var tip = User.FindFirst("tip")?.Value;
            var idClaim = User.FindFirst("id")?.Value;

            // Predavac može dodati obavijest samo za svoj kolegij
            if (tip == "Predavac" && idClaim != null)
            {
                var predavacId = int.Parse(idClaim);
                var kolegij = await _kolegijService.GetById(obavijest.KolegijId);

                if (kolegij == null)
                    return NotFound("Kolegij nije pronađen.");

                var jePredavac = kolegij.Predavaci.Any(p => p.Id == predavacId);
                if (!jePredavac)
                    return Forbid();
            }

            obavijest.KreatorId = int.Parse(User.FindFirst("id")!.Value);
            var nova = await _obavijestService.Create(obavijest);
            return CreatedAtAction(nameof(GetById), new { id = nova.Id }, nova);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Obavijest obavijest)
        {
            var tip = User.FindFirst("tip")?.Value;
            var idClaim = User.FindFirst("id")?.Value;

            // Predavac može urediti samo svoju obavijest
            if (tip == "Predavac" && idClaim != null)
            {
                var existing = await _obavijestService.GetById(id);
                if (existing == null)
                    return NotFound("Obavijest nije pronađena.");

                var kreatorIme = existing.KreatorIme;
                var predavacId = int.Parse(idClaim);
                var kolegij = await _kolegijService.GetById(existing.KolegijId);

                var jePredavac = kolegij?.Predavaci.Any(p => p.Id == predavacId) ?? false;
                if (!jePredavac)
                    return Forbid();
            }

            var success = await _obavijestService.Update(id, obavijest);
            if (!success)
                return NotFound("Obavijest nije pronađena.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tip = User.FindFirst("tip")?.Value;
            var idClaim = User.FindFirst("id")?.Value;

            // Predavac može brisati samo svoju obavijest
            if (tip == "Predavac" && idClaim != null)
            {
                var existing = await _obavijestService.GetById(id);
                if (existing == null)
                    return NotFound("Obavijest nije pronađena.");

                var predavacId = int.Parse(idClaim);
                var kolegij = await _kolegijService.GetById(existing.KolegijId);

                var jePredavac = kolegij?.Predavaci.Any(p => p.Id == predavacId) ?? false;
                if (!jePredavac)
                    return Forbid();
            }

            var success = await _obavijestService.Delete(id);
            if (!success)
                return NotFound("Obavijest nije pronađena.");

            return NoContent();
        }
    }
}