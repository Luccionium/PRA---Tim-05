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

    public async Task<IActionResult> Create()
    {
        var kolegiji = await _api.GetKolegijiAsync();
        return View(new ObavijestFormViewModel { Kolegiji = kolegiji });
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
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
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
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _api.DeleteObavijestAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
