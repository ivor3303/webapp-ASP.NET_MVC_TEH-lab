using System.Text.Json;
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

    public AiAsistentController(
        AiService aiService,
        EFRadnaOpremaRepository radnaOpremaRepository,
        VjezbaDbContext dbContext)
    {
        _aiService = aiService;
        _radnaOpremaRepository = radnaOpremaRepository;
        _dbContext = dbContext;
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
            return BadRequest(new { error = "Tekst je obavezan." });
        }

        var aiResponse = await _aiService.ParseOpremaFromText(request.Text.Trim());
        var parsed = TryParseAiResponse(aiResponse);

        return Json(new
        {
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

    private static ParsedAiOprema TryParseAiResponse(string aiResponse)
    {
        try
        {
            var candidate = aiResponse.Trim();
            var firstBrace = candidate.IndexOf('{');
            var lastBrace = candidate.LastIndexOf('}');

            if (firstBrace >= 0 && lastBrace > firstBrace)
            {
                candidate = candidate.Substring(firstBrace, lastBrace - firstBrace + 1);
            }

            var parsed = JsonSerializer.Deserialize<ParsedAiOprema>(candidate, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (parsed is null)
            {
                return new ParsedAiOprema();
            }

            return parsed;
        }
        catch
        {
            return new ParsedAiOprema();
        }
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

    public class ParsedAiOprema
    {
        public string Naziv { get; set; } = string.Empty;
        public string InventarniBroj { get; set; } = string.Empty;
        public string SerijskiBroj { get; set; } = string.Empty;
        public int LokacijaId { get; set; }
        public int ProizvodacId { get; set; }
        public int KategorijaOpremeId { get; set; }
    }
}
