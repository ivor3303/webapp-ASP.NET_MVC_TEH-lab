using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.App.Data;
using Vjezba.App.Repositories.EF;
using Vjezba.App.Services;
using Vjezba.App.ViewModels;
using Vjezba.Model;

namespace Vjezba.App.Controllers;

[AllowAnonymous]
[Route("ai-asistent")]
public class AiAsistentController : Controller
{
    private readonly AiService _aiService;
    private readonly EFRadnaOpremaRepository _radnaOpremaRepository;
    private readonly VjezbaDbContext _dbContext;
    private readonly ILogger<AiAsistentController> _logger;

    public AiAsistentController(
        AiService aiService,
        EFRadnaOpremaRepository radnaOpremaRepository,
        VjezbaDbContext dbContext,
        ILogger<AiAsistentController> logger)
    {
        _aiService = aiService;
        _radnaOpremaRepository = radnaOpremaRepository;
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        return View(new AiAsistentViewModel());
    }

    [HttpPost("parse")]
    public async Task<IActionResult> Parse([FromBody] ParseAiRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Text))
        {
            return Json(new { success = false, error = "Tekst je obavezan." });
        }

        if (!_aiService.IsConfigured)
        {
            _logger.LogWarning("AI parse odbijen: Anthropic:ApiKey nije postavljen na serveru.");
            return Json(new { success = false, error = "Anthropic API ključ nije postavljen na serveru (provjerite Railway varijable)." });
        }

        ParsedAiOprema parsed;
        try
        {
            parsed = await _aiService.ParseOpremaFromText(request.Text.Trim());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Neočekivana greška prilikom AI parsiranja teksta.");
            return Json(new { success = false, error = "Greška poziva AI servisa: " + ex.Message });
        }

        var hasData = !string.IsNullOrWhiteSpace(parsed.Naziv)
            || !string.IsNullOrWhiteSpace(parsed.InventarniBroj)
            || !string.IsNullOrWhiteSpace(parsed.SerijskiBroj);

        if (!hasData)
        {
            return Json(new { success = false, error = "AI nije uspio prepoznati podatke iz teksta. Pokušajte s detaljnijim opisom." });
        }

        return Json(new
        {
            success = true,
            naziv = parsed.Naziv,
            inventarniBroj = parsed.InventarniBroj,
            serijskiBroj = parsed.SerijskiBroj,
            lokacijaId = parsed.LokacijaId,
            proizvodacId = parsed.ProizvodacId,
            kategorijaOpremeId = parsed.KategorijaOpremeId
        });
    }

    [HttpPost("spremi")]
    [ValidateAntiForgeryToken]
    public IActionResult Spremi(AiAsistentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "AI Asistent - Unos podataka";
            return View("Index", model);
        }

        var lokacijaId = ResolveLokacijaId(model.LokacijaId);
        var proizvodacId = ResolveProizvodacId(model.ProizvodacId);
        var kategorijaId = ResolveKategorijaId(model.KategorijaOpremeId);

        if (lokacijaId == 0 || proizvodacId == 0 || kategorijaId == 0)
        {
            ModelState.AddModelError(string.Empty, "Nije moguće spremiti opremu jer nedostaju povezani podaci (lokacija, proizvođač ili kategorija).");
            ViewData["Title"] = "AI Asistent - Unos podataka";
            return View("Index", model);
        }

        var entity = new RadnaOprema
        {
            Naziv = model.Naziv.Trim(),
            InventarniBroj = model.InventarniBroj.Trim(),
            SerijskiBroj = model.SerijskiBroj.Trim(),
            DatumNabave = DateTime.UtcNow,
            Status = StatusOpreme.Ispravna,
            LokacijaId = lokacijaId,
            ProizvodacId = proizvodacId,
            KategorijaOpremeId = kategorijaId,
            DeletedAt = null
        };

        _radnaOpremaRepository.Create(entity);

        TempData["Success"] = "Radna oprema je uspješno kreirana putem AI asistenta.";
        return RedirectToAction("Details", "RadnaOprema", new { id = entity.Id });
    }

    private int ResolveLokacijaId(int preferredId)
    {
        if (preferredId > 0 && _dbContext.Lokacije.Any(x => x.Id == preferredId && x.DeletedAt == null))
        {
            return preferredId;
        }

        return _dbContext.Lokacije
            .Where(x => x.DeletedAt == null)
            .OrderBy(x => x.Id)
            .Select(x => x.Id)
            .FirstOrDefault();
    }

    private int ResolveProizvodacId(int preferredId)
    {
        if (preferredId > 0 && _dbContext.Proizvodaci.Any(x => x.Id == preferredId && x.DeletedAt == null))
        {
            return preferredId;
        }

        return _dbContext.Proizvodaci
            .Where(x => x.DeletedAt == null)
            .OrderBy(x => x.Id)
            .Select(x => x.Id)
            .FirstOrDefault();
    }

    private int ResolveKategorijaId(int preferredId)
    {
        if (preferredId > 0 && _dbContext.KategorijeOpreme.Any(x => x.Id == preferredId && x.DeletedAt == null))
        {
            return preferredId;
        }

        return _dbContext.KategorijeOpreme
            .Where(x => x.DeletedAt == null)
            .OrderBy(x => x.Id)
            .Select(x => x.Id)
            .FirstOrDefault();
    }

    public class ParseAiRequest
    {
        public string Text { get; set; } = string.Empty;
    }
}
