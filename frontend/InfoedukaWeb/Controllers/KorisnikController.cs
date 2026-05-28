using InfoedukaWeb.Models;
using InfoedukaWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfoedukaWeb.Controllers;

[Route("Predavaci")]
[Authorize(Roles = "Admin")]
public class KorisnikController : Controller
{
    private readonly ApiService _api;

    public KorisnikController(ApiService api) => _api = api;

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var korisnici = await _api.GetKorisniciAsync();
        return View(korisnici);
    }

    [HttpGet("Create")]
    public IActionResult Create() => View(new KorisnikFormViewModel());

    [HttpPost("Create")]
    public async Task<IActionResult> Create(KorisnikFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        await _api.CreateKorisnikAsync(model.Ime, model.Prezime, model.Email, model.Lozinka, model.Tip);
        TempData["Toast"] = "Korisnik je dodan.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var korisnik = await _api.GetKorisnikAsync(id);
        if (korisnik == null) return NotFound();
        return View(new KorisnikFormViewModel
        {
            Id = korisnik.Id,
            Ime = korisnik.Ime,
            Prezime = korisnik.Prezime,
            Email = korisnik.Email,
            Tip = korisnik.Tip
        });
    }

    [HttpPost("Edit")]
    public async Task<IActionResult> Edit(KorisnikFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        await _api.UpdateKorisnikAsync(model.Id, model.Ime, model.Prezime, model.Email, model.Lozinka, model.Tip);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _api.DeleteKorisnikAsync(id);
        TempData["Toast"] = "Korisnik je obrisan.";
        return RedirectToAction(nameof(Index));
    }
}
