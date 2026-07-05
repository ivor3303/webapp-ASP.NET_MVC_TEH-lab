using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.App.Data;
using Vjezba.App.DTOs;
using Vjezba.Model;

namespace Vjezba.App.Controllers.Api;

[ApiController]
[Route("api/radnaOprema")]
public class RadnaOpremaApiController : ControllerBase
{
    private readonly VjezbaDbContext _context;
    private readonly ILogger<RadnaOpremaApiController> _logger;

    public RadnaOpremaApiController(VjezbaDbContext context, ILogger<RadnaOpremaApiController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RadnaOpremaDTO>>> GetAll([FromQuery] string? q = null)
    {
        _logger.LogInformation("Fetching all RadnaOprema");
        var query = _context.RadnaOprema
            .Include(x => x.Lokacija)
            .Include(x => x.Proizvodac)
            .Include(x => x.Kategorija)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var normalizedQuery = q.Trim();
            query = query.Where(x => x.Naziv.Contains(normalizedQuery)
                || x.InventarniBroj.Contains(normalizedQuery));
        }

        var items = await query.Where(x => x.DeletedAt == null).ToListAsync();
        return Ok(items.Select(ToDTO));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RadnaOpremaDTO>> GetById(int id)
    {
        _logger.LogInformation("Fetching RadnaOprema with id {Id}", id);
        var item = await _context.RadnaOprema
            .Include(x => x.Lokacija)
            .Include(x => x.Proizvodac)
            .Include(x => x.Kategorija)
            .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);

        if (item is null)
        {
            _logger.LogWarning("RadnaOprema with id {Id} not found", id);
            return NotFound();
        }

        return Ok(ToDTO(item));
    }

    [HttpPost]
    public async Task<ActionResult<RadnaOpremaDTO>> Create([FromBody] RadnaOprema model)
    {
        _logger.LogInformation("Creating new RadnaOprema");
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        model.DeletedAt = null;
        _context.RadnaOprema.Add(model);
        await _context.SaveChangesAsync();

        await _context.Entry(model).Reference(x => x.Lokacija).LoadAsync();
        await _context.Entry(model).Reference(x => x.Proizvodac).LoadAsync();
        await _context.Entry(model).Reference(x => x.Kategorija).LoadAsync();

        var dto = ToDTO(model);
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] RadnaOprema model)
    {
        _logger.LogInformation("Updating RadnaOprema with id {Id}", id);
        var item = await _context.RadnaOprema.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        item.Naziv = model.Naziv;
        item.InventarniBroj = model.InventarniBroj;
        item.SerijskiBroj = model.SerijskiBroj;
        item.DatumNabave = model.DatumNabave;
        item.Status = model.Status;
        item.LokacijaId = model.LokacijaId;
        item.ProizvodacId = model.ProizvodacId;
        item.KategorijaOpremeId = model.KategorijaOpremeId;

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Deleting RadnaOprema with id {Id}", id);
        var item = await _context.RadnaOprema.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        item.Status = StatusOpreme.Otpisana;
        await _context.SaveChangesAsync();
        return Ok();
    }

    private RadnaOpremaDTO ToDTO(RadnaOprema model)
    {
        return new RadnaOpremaDTO
        {
            Id = model.Id,
            Naziv = model.Naziv,
            InventarniBroj = model.InventarniBroj,
            SerijskiBroj = model.SerijskiBroj,
            DatumNabave = model.DatumNabave,
            Status = model.Status.ToString(),
            Lokacija = model.Lokacija is null ? null : new LokacijaDTO
            {
                Id = model.Lokacija.Id,
                Naziv = model.Lokacija.Naziv,
                Adresa = model.Lokacija.Adresa
            },
            Proizvodac = model.Proizvodac is null ? null : new ProizvodacDTO
            {
                Id = model.Proizvodac.Id,
                Naziv = model.Proizvodac.Naziv,
                Drzava = model.Proizvodac.Drzava,
                KontaktEmail = model.Proizvodac.KontaktEmail
            },
            Kategorija = model.Kategorija is null ? null : new KategorijaOpremeDTO
            {
                Id = model.Kategorija.Id,
                Naziv = model.Kategorija.Naziv,
                Opis = model.Kategorija.Opis
            }
        };
    }
}