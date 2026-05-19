using Microsoft.AspNetCore.Mvc;
using Vjezba.App.Repositories.EF;
using Vjezba.Model;

namespace Vjezba.App.Controllers;

[Route("oprema")]
public class RadnaOpremaController : Controller
{
    private readonly EFRadnaOpremaRepository _repository;
    private readonly EFLokacijaRepository _lokacijaRepository;
    private readonly EFProizvodacRepository _proizvodacRepository;
    private readonly EFKategorijaOpremeRepository _kategorijaOpremeRepository;

    public RadnaOpremaController(
        EFRadnaOpremaRepository repository,
        EFLokacijaRepository lokacijaRepository,
        EFProizvodacRepository proizvodacRepository,
        EFKategorijaOpremeRepository kategorijaOpremeRepository)
    {
        _repository = repository;
        _lokacijaRepository = lokacijaRepository;
        _proizvodacRepository = proizvodacRepository;
        _kategorijaOpremeRepository = kategorijaOpremeRepository;
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
                    || x.InventarniBroj.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase)
                    || x.SerijskiBroj.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return PartialView("_RadnaOpremaList", items);
    }

    [HttpGet]
    [Route("autocomplete")]
    public IActionResult SearchAutocomplete(string query)
    {
        var normalizedQuery = query?.Trim() ?? string.Empty;

        var results = _repository.GetAll()
            .Where(x => x.DeletedAt == null)
            .Where(x => string.IsNullOrWhiteSpace(normalizedQuery)
                || x.Naziv.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Json(results.Select(x => new { id = x.Id, text = x.Naziv }));
    }

    [HttpGet]
    [Route("novi")]
    public IActionResult Create()
    {
        return View(new RadnaOprema { DatumNabave = DateTime.UtcNow });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("novi")]
    public IActionResult Create(RadnaOprema model)
    {
        if (!ModelState.IsValid)
        {
            HydrateAutocompleteSelections(model);
            return View(model);
        }

        model.DeletedAt = null;
        _repository.Create(model);
        TempData["Success"] = "Radna oprema je uspješno dodana.";
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
    public IActionResult Edit(int id, RadnaOprema model)
    {
        var item = _repository.GetById(id);
        if (item is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.Id = id;
            HydrateAutocompleteSelections(model);
            return View(model);
        }

        item.Naziv = model.Naziv;
        item.InventarniBroj = model.InventarniBroj;
        item.SerijskiBroj = model.SerijskiBroj;
        item.DatumNabave = model.DatumNabave;
        item.Status = model.Status;
        item.LokacijaId = model.LokacijaId;
        item.ProizvodacId = model.ProizvodacId;
        item.KategorijaOpremeId = model.KategorijaOpremeId;

        _repository.Update(item);
        TempData["Success"] = "Radna oprema je uspješno ažurirana.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("obrisi/{id:int}")]
    public IActionResult Delete(int id)
    {
        _repository.Delete(id);
        TempData["Success"] = "Radna oprema je uspješno obrisana.";
        return RedirectToAction(nameof(Index));
    }

    private void HydrateAutocompleteSelections(RadnaOprema model)
    {
        model.Lokacija = _lokacijaRepository.GetById(model.LokacijaId);
        model.Proizvodac = _proizvodacRepository.GetById(model.ProizvodacId);
        model.Kategorija = _kategorijaOpremeRepository.GetById(model.KategorijaOpremeId);
    }
}
