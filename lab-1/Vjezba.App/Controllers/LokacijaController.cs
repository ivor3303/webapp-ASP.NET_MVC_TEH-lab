using Microsoft.AspNetCore.Mvc;
using Vjezba.App.Repositories.EF;
using Vjezba.Model;

namespace Vjezba.App.Controllers;

[Route("lokacije")]
public class LokacijaController : Controller
{
    private readonly EFLokacijaRepository _repository;

    public LokacijaController(EFLokacijaRepository repository)
    {
        _repository = repository;
    }

    [Route("")]
    public IActionResult Index()
    {
        var items = _repository.GetAll();
        return View(items);
    }

    [Route("detalji/{id:int}")]
    public IActionResult Details(int id)
    {
        var item = _repository.GetById(id);
        if (item is null)
        {
            return NotFound();
        }

        return View(item);
    }

    [HttpGet]
    [Route("search")]
    public IActionResult Search(string? query)
    {
        var items = _repository.GetAll();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var normalizedQuery = query.Trim();
            items = items
                .Where(x => x.Naziv.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase)
                    || x.Adresa.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return PartialView("_LokacijaList", items);
    }

    [HttpGet]
    [Route("novi")]
    public IActionResult Create()
    {
        return View(new Lokacija());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("novi")]
    public IActionResult Create(Lokacija model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        model.DeletedAt = null;
        _repository.Create(model);
        TempData["Success"] = "Lokacija je uspješno dodana.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Route("uredi/{id:int}")]
    public IActionResult Edit(int id)
    {
        var item = _repository.GetById(id);
        if (item is null)
        {
            return NotFound();
        }

        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("uredi/{id:int}")]
    public IActionResult Edit(int id, Lokacija model)
    {
        var item = _repository.GetById(id);
        if (item is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.Id = id;
            return View(model);
        }

        item.Naziv = model.Naziv;
        item.Adresa = model.Adresa;

        _repository.Update(item);
        TempData["Success"] = "Lokacija je uspješno ažurirana.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("obrisi/{id:int}")]
    public IActionResult Delete(int id)
    {
        _repository.Delete(id);
        TempData["Success"] = "Lokacija je uspješno obrisana.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("/lokacije/autocomplete")]
    public IActionResult Autocomplete(string? query)
    {
        var results = _repository.GetAll();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var normalizedQuery = query.Trim();
            results = results
                .Where(l => l.Naziv.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return Json(results.Select(l => new { id = l.Id, text = l.Naziv }));
    }
}
