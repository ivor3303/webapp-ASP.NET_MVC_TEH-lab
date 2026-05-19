using Microsoft.AspNetCore.Mvc;
using Vjezba.App.Repositories.EF;
using Vjezba.Model;

namespace Vjezba.App.Controllers;

[Route("zaduzenjaopreme")]
public class ZaduzenjeOpremeController : Controller
{
    private readonly EFZaduzenjeOpremeRepository _repository;
    private readonly EFRadnikRepository _radnikRepository;
    private readonly EFRadnaOpremaRepository _radnaOpremaRepository;

    public ZaduzenjeOpremeController(EFZaduzenjeOpremeRepository repository, EFRadnikRepository radnikRepository, EFRadnaOpremaRepository radnaOpremaRepository)
    {
        _repository = repository;
        _radnikRepository = radnikRepository;
        _radnaOpremaRepository = radnaOpremaRepository;
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
                .Where(x => x.Radnik is not null && $"{x.Radnik.Ime} {x.Radnik.Prezime}".Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase)
                    || x.RadnaOprema?.Naziv.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase) == true)
                .ToList();
        }

        return PartialView("_ZaduzenjeOpremeList", items);
    }

    [HttpGet]
    [Route("novi")]
    public IActionResult Create()
    {
        return View(new ZaduzenjeOpreme { DatumZaduzenja = DateTime.UtcNow });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("novi")]
    public IActionResult Create(ZaduzenjeOpreme model)
    {
        if (model.RadnikId == 0)
        {
            ModelState.AddModelError(nameof(model.RadnikId), "Molimo odaberite radnika");
        }

        if (model.RadnaOpremaId == 0)
        {
            ModelState.AddModelError(nameof(model.RadnaOpremaId), "Molimo odaberite opremu");
        }

        if (!ModelState.IsValid)
        {
            HydrateSelections(model);
            return View(model);
        }

        model.DeletedAt = null;
        _repository.Create(model);
        TempData["Success"] = "Zaduženje opreme je uspješno dodano.";
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

        ViewBag.RadnikText = item.Radnik is null ? string.Empty : $"{item.Radnik.Ime} {item.Radnik.Prezime}";
        ViewBag.RadnaOpremaText = item.RadnaOprema?.Naziv ?? string.Empty;

        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("uredi/{id:int}")]
    public IActionResult Edit(int id, ZaduzenjeOpreme model)
    {
        var item = _repository.GetById(id);
        if (item is null)
        {
            return NotFound();
        }

        if (model.RadnikId == 0)
        {
            ModelState.AddModelError(nameof(model.RadnikId), "Molimo odaberite radnika");
        }

        if (model.RadnaOpremaId == 0)
        {
            ModelState.AddModelError(nameof(model.RadnaOpremaId), "Molimo odaberite opremu");
        }

        if (!ModelState.IsValid)
        {
            model.Id = id;
            HydrateSelections(model);
            return View(model);
        }

        item.RadnikId = model.RadnikId;
        item.RadnaOpremaId = model.RadnaOpremaId;
        item.DatumZaduzenja = model.DatumZaduzenja;
        item.DatumRazduzenja = model.DatumRazduzenja;

        _repository.Update(item);
        TempData["Success"] = "Zaduženje opreme je uspješno ažurirano.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("obrisi/{id:int}")]
    public IActionResult Delete(int id)
    {
        _repository.Delete(id);
        TempData["Success"] = "Zaduženje opreme je uspješno obrisano.";
        return RedirectToAction(nameof(Index));
    }

    private void HydrateSelections(ZaduzenjeOpreme model)
    {
        model.Radnik = _radnikRepository.GetById(model.RadnikId);
        model.RadnaOprema = _radnaOpremaRepository.GetById(model.RadnaOpremaId);
    }
}
