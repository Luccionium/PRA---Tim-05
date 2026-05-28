using InfoedukaWeb.Models;
using InfoedukaWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfoedukaWeb.Controllers;

[Route("predavaci/[action]/{id?}")]
[Authorize(Roles = "Admin")]
public class KorisnikController : Controller
{
    private readonly ApiService _api;

    public KorisnikController(ApiService api) => _api = api;

    public async Task<IActionResult> Index()
    {
        var korisnici = await _api.GetKorisniciAsync();
        return View(korisnici);
    }

    public IActionResult Create() => View(new KorisnikFormViewModel());

    [HttpPost]
    public async Task<IActionResult> Create(KorisnikFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        await _api.CreateKorisnikAsync(model.Ime, model.Prezime, model.Email, model.Lozinka, model.Tip);
        return RedirectToAction(nameof(Index));
    }

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

    [HttpPost]
    public async Task<IActionResult> Edit(KorisnikFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        await _api.UpdateKorisnikAsync(model.Id, model.Ime, model.Prezime, model.Email, model.Lozinka, model.Tip);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _api.DeleteKorisnikAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
