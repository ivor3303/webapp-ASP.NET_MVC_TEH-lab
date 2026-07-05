using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vjezba.App.Repositories.EF;
using Vjezba.Model;

namespace Vjezba.App.Controllers;

[Route("odrzavanja")]
public class OdrzavanjeController : Controller
{
    private readonly EFOdrzavanjeRepository _repository;
    private readonly EFRadnaOpremaRepository _radnaOpremaRepository;
    private readonly EFRadnikRepository _radnikRepository;

    public OdrzavanjeController(EFOdrzavanjeRepository repository, EFRadnaOpremaRepository radnaOpremaRepository, EFRadnikRepository radnikRepository)
    {
        _repository = repository;
        _radnaOpremaRepository = radnaOpremaRepository;
        _radnikRepository = radnikRepository;
    }

    [Route("")]
    [AllowAnonymous]
    public IActionResult Index()
    {
        var items = _repository.GetAll();
        return View(items);
    }

    [Route("zapis/{id:int}")]
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
                .Where(x => x.Opis.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase)
                    || x.Napomena.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase)
                    || x.Oprema?.Naziv.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase) == true
                    || x.Izvrsio is not null && $"{x.Izvrsio.Ime} {x.Izvrsio.Prezime}".Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return PartialView("_OdrzavanjeList", items);
    }

    [HttpGet]
    [Route("novi")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Create()
    {
        return View(new Odrzavanje { Datum = DateTime.UtcNow });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("novi")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Create(Odrzavanje model)
    {
        if (model.OpremaId == 0)
        {
            ModelState.AddModelError(nameof(model.OpremaId), "Molimo odaberite opremu");
        }

        if (model.IzvrsioId == 0)
        {
            ModelState.AddModelError(nameof(model.IzvrsioId), "Molimo odaberite radnika");
        }

        if (!ModelState.IsValid)
        {
            HydrateSelections(model);
            return View(model);
        }

        model.DeletedAt = null;
        _repository.Create(model);
        TempData["Success"] = "Održavanje je uspješno dodano.";
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
        ViewBag.IzvrsioText = item.Izvrsio is null ? string.Empty : $"{item.Izvrsio.Ime} {item.Izvrsio.Prezime}";

        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("uredi/{id:int}")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Edit(int id, Odrzavanje model)
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

        if (model.IzvrsioId == 0)
        {
            ModelState.AddModelError(nameof(model.IzvrsioId), "Molimo odaberite radnika");
        }

        if (!ModelState.IsValid)
        {
            model.Id = id;
            HydrateSelections(model);
            return View(model);
        }

        item.Datum = model.Datum;
        item.Opis = model.Opis;
        item.Cijena = model.Cijena;
        item.Napomena = model.Napomena;
        item.TrajanjeTicks = model.TrajanjeTicks;
        item.OpremaId = model.OpremaId;
        item.IzvrsioId = model.IzvrsioId;

        _repository.Update(item);
        TempData["Success"] = "Održavanje je uspješno ažurirano.";
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
        TempData["Success"] = "Održavanje je uspješno obrisano.";
        return RedirectToAction(nameof(Index));
    }

    private void HydrateSelections(Odrzavanje model)
    {
        model.Oprema = _radnaOpremaRepository.GetById(model.OpremaId);
        model.Izvrsio = _radnikRepository.GetById(model.IzvrsioId);
    }
}
