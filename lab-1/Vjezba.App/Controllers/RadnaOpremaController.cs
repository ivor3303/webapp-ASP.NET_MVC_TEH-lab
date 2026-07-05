using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vjezba.App.Data;
using Vjezba.App.Repositories.EF;
using Vjezba.Model;

namespace Vjezba.App.Controllers;

[Route("oprema")]
public class RadnaOpremaController : Controller
{
    private readonly EFRadnaOpremaRepository _repository;
    private readonly VjezbaDbContext _dbContext;
    private readonly EFLokacijaRepository _lokacijaRepository;
    private readonly EFProizvodacRepository _proizvodacRepository;
    private readonly EFKategorijaOpremeRepository _kategorijaOpremeRepository;

    public RadnaOpremaController(
        EFRadnaOpremaRepository repository,
        VjezbaDbContext dbContext,
        EFLokacijaRepository lokacijaRepository,
        EFProizvodacRepository proizvodacRepository,
        EFKategorijaOpremeRepository kategorijaOpremeRepository)
    {
        _repository = repository;
        _dbContext = dbContext;
        _lokacijaRepository = lokacijaRepository;
        _proizvodacRepository = proizvodacRepository;
        _kategorijaOpremeRepository = kategorijaOpremeRepository;
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
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Create()
    {
        return View(new RadnaOprema { DatumNabave = DateTime.UtcNow });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("novi")]
    [Authorize(Roles = "Admin,Manager")]
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
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        _repository.Delete(id);
        TempData["Success"] = "Radna oprema je uspješno obrisana.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    [Route("oprema/{opremaId:int}/upload")]
    public IActionResult UploadAttachment(int opremaId, IFormFile file)
    {
        var oprema = _dbContext.RadnaOprema.FirstOrDefault(o => o.Id == opremaId);
        if (oprema == null) return NotFound();
        if (file == null || file.Length == 0) return BadRequest();

        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "oprema", opremaId.ToString());
        Directory.CreateDirectory(uploadsPath);

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadsPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        var attachment = new Attachment
        {
            RadnaOpremaId = opremaId,
            FileName = file.FileName,
            FilePath = "/uploads/oprema/" + opremaId + "/" + fileName,
            ContentType = file.ContentType,
            FileSize = file.Length,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Attachments.Add(attachment);
        _dbContext.SaveChanges();

        return Json(new { success = true });
    }

    [Route("oprema/{opremaId:int}/attachments")]
    public IActionResult GetAttachments(int opremaId)
    {
        var attachments = _dbContext.Attachments
            .Where(a => a.RadnaOpremaId == opremaId)
            .OrderByDescending(a => a.CreatedAt)
            .ToList();

        return PartialView("_AttachmentList", attachments);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    [Route("oprema/attachment/delete/{id:int}")]
    public IActionResult DeleteAttachment(int id)
    {
        var attachment = _dbContext.Attachments.FirstOrDefault(a => a.Id == id);
        if (attachment == null) return NotFound();

        var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", attachment.FilePath.TrimStart('/'));
        if (System.IO.File.Exists(physicalPath))
        {
            System.IO.File.Delete(physicalPath);
        }

        _dbContext.Attachments.Remove(attachment);
        _dbContext.SaveChanges();

        return Json(new { success = true });
    }

    private void HydrateAutocompleteSelections(RadnaOprema model)
    {
        model.Lokacija = _lokacijaRepository.GetById(model.LokacijaId);
        model.Proizvodac = _proizvodacRepository.GetById(model.ProizvodacId);
        model.Kategorija = _kategorijaOpremeRepository.GetById(model.KategorijaOpremeId);
    }
}
