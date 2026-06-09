using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vjezba.App.Repositories.EF;
using Vjezba.App.ViewModels;
using Vjezba.Model;

namespace Vjezba.App.Controllers;

[Route("proizvodaci")]
public class ProizvodacController : Controller
{
    private readonly EFProizvodacRepository _repository;
    private readonly EFRadnaOpremaRepository _radnaOpremaRepository;

    public ProizvodacController(EFProizvodacRepository repository, EFRadnaOpremaRepository radnaOpremaRepository)
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
        var proizvodac = _repository.GetById(id);
        if (proizvodac is null)
        {
            return NotFound();
        }

        var model = new ProizvodacDetailsViewModel
        {
            Proizvodac = proizvodac,
            Oprema = _radnaOpremaRepository
                .GetAll()
                .Where(o => o.Proizvodac.Id == proizvodac.Id)
                .ToList()
        };

        return View(model);
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
                    || x.Drzava.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase)
                    || x.KontaktEmail.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return PartialView("_ProizvodacList", items);
    }

    [HttpGet]
    [Route("novi")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Create()
    {
        return View(new Proizvodac());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("novi")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Create(Proizvodac model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        model.DeletedAt = null;
        _repository.Create(model);
        TempData["Success"] = "Proizvođač je uspješno dodan.";
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
    public IActionResult Edit(int id, Proizvodac model)
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
        item.Drzava = model.Drzava;
        item.KontaktEmail = model.KontaktEmail;

        _repository.Update(item);
        TempData["Success"] = "Proizvođač je uspješno ažuriran.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("obrisi/{id:int}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        _repository.Delete(id);
        TempData["Success"] = "Proizvođač je uspješno obrisan.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("/proizvodac/autocomplete")]
    public IActionResult Autocomplete(string? query)
    {
        var results = _repository.GetAll();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var normalizedQuery = query.Trim();
            results = results
                .Where(p => p.Naziv.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return Json(results.Select(p => new { id = p.Id, text = p.Naziv }));
    }
}
