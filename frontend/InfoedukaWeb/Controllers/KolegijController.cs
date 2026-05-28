using InfoedukaWeb.Models;
using InfoedukaWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfoedukaWeb.Controllers;

[Authorize]
public class KolegijController : Controller
{
    private readonly ApiService _api;

    public KolegijController(ApiService api) => _api = api;

    public async Task<IActionResult> Index()
    {
        var kolegiji = await _api.GetKolegijiAsync();
        return View(kolegiji);
    }

    public async Task<IActionResult> Details(int id)
    {
        var kolegij = await _api.GetKolegijAsync(id);
        if (kolegij == null) return NotFound();

        var obavijesti = await _api.GetObavijestiByKolegijAsync(id);
        var sviPredavaci = User.IsInRole("Admin") ? await _api.GetKorisniciAsync() : new();

        return View(new KolegijDetailsViewModel
        {
            Kolegij = kolegij,
            Obavijesti = obavijesti,
            SviPredavaci = sviPredavaci.Where(k => k.Tip == "Predavac").ToList()
        });
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create()
    {
        var predavaci = await _api.GetKorisniciAsync();
        return View(new KolegijFormViewModel { SviPredavaci = predavaci.Where(k => k.Tip == "Predavac").ToList() });
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(KolegijFormViewModel model)
    {
        if (!model.SelectedPredavaciIds.Any())
            ModelState.AddModelError("SelectedPredavaciIds", "Morate odabrati barem jednog predavača.");

        if (!ModelState.IsValid)
        {
            var predavaci = await _api.GetKorisniciAsync();
            model.SviPredavaci = predavaci.Where(k => k.Tip == "Predavac").ToList();
            return View(model);
        }

        var kolegij = await _api.CreateKolegijAsync(model.Naziv, model.Opis);
        if (kolegij != null)
            foreach (var predavacId in model.SelectedPredavaciIds)
                await _api.AddPredavacAsync(kolegij.Id, predavacId);

        TempData["Toast"] = "Kolegij je dodan.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var kolegij = await _api.GetKolegijAsync(id);
        if (kolegij == null) return NotFound();
        var predavaci = await _api.GetKorisniciAsync();
        return View(new KolegijFormViewModel
        {
            Id = kolegij.Id,
            Naziv = kolegij.Naziv,
            Opis = kolegij.Opis,
            SelectedPredavaciIds = kolegij.Predavaci.Select(p => p.Id).ToList(),
            SviPredavaci = predavaci.Where(k => k.Tip == "Predavac").ToList()
        });
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(KolegijFormViewModel model)
    {
        if (!model.SelectedPredavaciIds.Any())
            ModelState.AddModelError("SelectedPredavaciIds", "Morate odabrati barem jednog predavača.");

        if (!ModelState.IsValid)
        {
            var predavaci = await _api.GetKorisniciAsync();
            model.SviPredavaci = predavaci.Where(k => k.Tip == "Predavac").ToList();
            return View(model);
        }

        await _api.UpdateKolegijAsync(model.Id, model.Naziv, model.Opis);

        var current = await _api.GetKolegijAsync(model.Id);
        var currentIds = current?.Predavaci.Select(p => p.Id).ToHashSet() ?? new();
        var selectedIds = model.SelectedPredavaciIds.ToHashSet();

        foreach (var predavacId in selectedIds.Except(currentIds))
            await _api.AddPredavacAsync(model.Id, predavacId);

        foreach (var predavacId in currentIds.Except(selectedIds))
            await _api.RemovePredavacAsync(model.Id, predavacId);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _api.DeleteKolegijAsync(id);
        TempData["Toast"] = "Kolegij je obrisan.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddPredavac(int kolegijId, int predavacId)
    {
        await _api.AddPredavacAsync(kolegijId, predavacId);
        return RedirectToAction(nameof(Details), new { id = kolegijId });
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemovePredavac(int kolegijId, int predavacId)
    {
        await _api.RemovePredavacAsync(kolegijId, predavacId);
        return RedirectToAction(nameof(Details), new { id = kolegijId });
    }
}
