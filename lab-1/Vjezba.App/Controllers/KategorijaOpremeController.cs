using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vjezba.App.Repositories.EF;
using Vjezba.Model;

namespace Vjezba.App.Controllers;

[Route("kategorijaopreme")]
public class KategorijaOpremeController : Controller
{
    private readonly EFKategorijaOpremeRepository _repository;

    public KategorijaOpremeController(EFKategorijaOpremeRepository repository)
    {
        _repository = repository;
    }

    [Route("")]
    [AllowAnonymous]
    public IActionResult Index()
    {
        var items = _repository.GetAll();
        return View(items);
    }

    [Route("detalji/{id:int}")]
    [AllowAnonymous]
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
                    || x.Opis.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return PartialView("_KategorijaOpremeList", items);
    }

    [HttpGet]
    [Route("novi")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Create()
    {
        return View(new KategorijaOpreme());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("novi")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Create(KategorijaOpreme model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        model.DeletedAt = null;
        _repository.Create(model);
        TempData["Success"] = "Kategorija opreme je uspješno dodana.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Route("uredi/{id:int}")]
    [Authorize(Roles = "Admin,Manager")]
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
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Edit(int id, KategorijaOpreme model)
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
        item.Opis = model.Opis;

        _repository.Update(item);
        TempData["Success"] = "Kategorija opreme je uspješno ažurirana.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("obrisi/{id:int}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var item = _repository.GetById(id);
        if (item is null)
        {
            return NotFound();
        }

        item.DeletedAt = DateTime.UtcNow;
        _repository.Update(item);
        TempData["Success"] = "Kategorija opreme je uspješno obrisana.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("/kategorijaopreme/autocomplete")]
    public IActionResult Autocomplete(string? query)
    {
        var results = _repository.GetAll();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var normalizedQuery = query.Trim();
            results = results
                .Where(k => k.Naziv.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return Json(results.Select(k => new { id = k.Id, text = k.Naziv }));
    }
}
