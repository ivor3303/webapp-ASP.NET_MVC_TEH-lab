using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vjezba.App.Repositories.EF;
using Vjezba.Model;

namespace Vjezba.App.Controllers;

[Route("servisnizahtjevi")]
public class ServisniZahtjevController : Controller
{
    private readonly EFServisniZahtjevRepository _repository;
    private readonly EFRadnaOpremaRepository _radnaOpremaRepository;

    public ServisniZahtjevController(EFServisniZahtjevRepository repository, EFRadnaOpremaRepository radnaOpremaRepository)
    {
        _repository = repository;
        _radnaOpremaRepository = radnaOpremaRepository;
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
                .Where(x => x.OpisKvara.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase)
                    || x.Komentar.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase)
                    || x.Oprema?.Naziv.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase) == true)
                .ToList();
        }

        return PartialView("_ServisniZahtjevList", items);
    }

    [HttpGet]
    [Route("novi")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Create()
    {
        return View(new ServisniZahtjev { DatumPrijave = DateTime.UtcNow });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("novi")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Create(ServisniZahtjev model)
    {
        if (model.OpremaId == 0)
        {
            ModelState.AddModelError(nameof(model.OpremaId), "Molimo odaberite opremu");
        }

        if (!ModelState.IsValid)
        {
            HydrateSelections(model);
            return View(model);
        }

        model.DeletedAt = null;
        _repository.Create(model);
        TempData["Success"] = "Servisni zahtjev je uspješno dodan.";
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

        ViewBag.OpremaText = item.Oprema?.Naziv ?? string.Empty;

        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("uredi/{id:int}")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Edit(int id, ServisniZahtjev model)
    {
        var item = _repository.GetById(id);
        if (item is null)
        {
            return NotFound();
        }

        if (model.OpremaId == 0)
        {
            ModelState.AddModelError(nameof(model.OpremaId), "Molimo odaberite opremu");
        }

        if (!ModelState.IsValid)
        {
            model.Id = id;
            HydrateSelections(model);
            return View(model);
        }

        item.DatumPrijave = model.DatumPrijave;
        item.OpisKvara = model.OpisKvara;
        item.Hitno = model.Hitno;
        item.Komentar = model.Komentar;
        item.OpremaId = model.OpremaId;

        _repository.Update(item);
        TempData["Success"] = "Servisni zahtjev je uspješno ažuriran.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("obrisi/{id:int}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        _repository.Delete(id);
        TempData["Success"] = "Servisni zahtjev je uspješno obrisan.";
        return RedirectToAction(nameof(Index));
    }

    private void HydrateSelections(ServisniZahtjev model)
    {
        model.Oprema = _radnaOpremaRepository.GetById(model.OpremaId);
    }
}
