using InfoedukaWeb.Models;
using InfoedukaWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfoedukaWeb.Controllers;

[Authorize]
public class ObavijestController : Controller
{
    private readonly ApiService _api;

    public ObavijestController(ApiService api) => _api = api;

    public async Task<IActionResult> Index()
    {
        var obavijesti = await _api.GetObavijestiAsync();
        return View(obavijesti);
    }

    public async Task<IActionResult> Create(int? kolegijId = null)
    {
        var kolegiji = await _api.GetKolegijiAsync();
        return View(new ObavijestFormViewModel
        {
            Kolegiji = kolegiji,
            KolegijId = kolegijId ?? 0,
            ReturnKolegijId = kolegijId
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(ObavijestFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Kolegiji = await _api.GetKolegijiAsync();
            return View(model);
        }
        await _api.CreateObavijestAsync(model.Naziv, model.Opis, model.DatumObjave, model.DatumIsteka, model.KolegijId);
        TempData["Toast"] = "Obavijest je dodana.";
        if (model.ReturnKolegijId.HasValue)
            return RedirectToAction("Details", "Kolegij", new { id = model.ReturnKolegijId.Value });
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, int? returnKolegijId = null)
    {
        var o = await _api.GetObavijestAsync(id);
        if (o == null) return NotFound();
        var kolegiji = await _api.GetKolegijiAsync();
        return View(new ObavijestFormViewModel
        {
            Id = o.Id,
            Naziv = o.Naziv,
            Opis = o.Opis,
            DatumObjave = o.DatumObjave,
            DatumIsteka = o.DatumIsteka,
            KolegijId = o.KolegijId,
            ReturnKolegijId = returnKolegijId,
            Kolegiji = kolegiji
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ObavijestFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Kolegiji = await _api.GetKolegijiAsync();
            return View(model);
        }
        await _api.UpdateObavijestAsync(model.Id, model.Naziv, model.Opis, model.DatumObjave, model.DatumIsteka, model.KolegijId);
        if (model.ReturnKolegijId.HasValue)
            return RedirectToAction("Details", "Kolegij", new { id = model.ReturnKolegijId.Value });
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id, int? returnKolegijId = null)
    {
        await _api.DeleteObavijestAsync(id);
        TempData["Toast"] = "Obavijest je obrisana.";
        if (returnKolegijId.HasValue)
            return RedirectToAction("Details", "Kolegij", new { id = returnKolegijId.Value });
        return RedirectToAction(nameof(Index));
    }
}
