using Microsoft.AspNetCore.Mvc;
using Vjezba.App.Repositories.EF;
using Vjezba.Model;

namespace Vjezba.App.Controllers;

[Route("radnici")]
public class RadnikController : Controller
{
    private readonly EFRadnikRepository _repository;

    public RadnikController(EFRadnikRepository repository)
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
                .Where(x => x.Ime.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase)
                    || x.Prezime.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase)
                    || x.RadnoMjesto.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase)
                    || x.Email.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase)
                    || x.Telefon.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return PartialView("_RadnikList", items);
    }

    [HttpGet]
    [Route("novi")]
    public IActionResult Create()
    {
        return View(new Radnik { DatumZaposlenja = DateTime.UtcNow, Aktivan = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("novi")]
    public IActionResult Create(Radnik model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        model.DeletedAt = null;
        _repository.Create(model);
        TempData["Success"] = "Radnik je uspješno dodan.";
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
    public IActionResult Edit(int id, Radnik model)
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

        item.Ime = model.Ime;
        item.Prezime = model.Prezime;
        item.RadnoMjesto = model.RadnoMjesto;
        item.Email = model.Email;
        item.Telefon = model.Telefon;
        item.DatumZaposlenja = model.DatumZaposlenja;
        item.Aktivan = model.Aktivan;

        _repository.Update(item);
        TempData["Success"] = "Radnik je uspješno ažuriran.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("obrisi/{id:int}")]
    public IActionResult Delete(int id)
    {
        _repository.Delete(id);
        TempData["Success"] = "Radnik je uspješno obrisan.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("/radnici/autocomplete")]
    public IActionResult Autocomplete(string? query)
    {
        var results = _repository.GetAll();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var normalizedQuery = query.Trim();
            results = results
                .Where(x => $"{x.Ime} {x.Prezime}".Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return Json(results.Select(x => new { id = x.Id, text = $"{x.Ime} {x.Prezime}" }));
    }
}
