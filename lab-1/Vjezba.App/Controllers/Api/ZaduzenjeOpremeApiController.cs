using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.App.Data;
using Vjezba.App.DTOs;
using Vjezba.Model;

namespace Vjezba.App.Controllers.Api;

[ApiController]
[Route("api/zaduzenjeOpreme")]
public class ZaduzenjeOpremeApiController : ControllerBase
{
    private readonly VjezbaDbContext _context;
    private readonly ILogger<ZaduzenjeOpremeApiController> _logger;

    public ZaduzenjeOpremeApiController(VjezbaDbContext context, ILogger<ZaduzenjeOpremeApiController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ZaduzenjeOpremeDTO>>> GetAll()
    {
        _logger.LogInformation("Fetching all ZaduzenjeOpreme");
        var items = await _context.ZaduzenjaOpreme
            .Include(x => x.Radnik)
            .Include(x => x.RadnaOprema)
            .Where(x => x.DeletedAt == null)
            .ToListAsync();
        return Ok(items.Select(ToDTO));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ZaduzenjeOpremeDTO>> GetById(int id)
    {
        _logger.LogInformation("Fetching ZaduzenjeOpreme with id {Id}", id);
        var item = await _context.ZaduzenjaOpreme
            .Include(x => x.Radnik)
            .Include(x => x.RadnaOprema)
            .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
        if (item is null)
        {
            _logger.LogWarning("ZaduzenjeOpreme with id {Id} not found", id);
            return NotFound();
        }
        return Ok(ToDTO(item));
    }

    [HttpPost]
    public async Task<ActionResult<ZaduzenjeOpremeDTO>> Create([FromBody] ZaduzenjeOpreme model)
    {
        _logger.LogInformation("Creating new ZaduzenjeOpreme");
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        model.DeletedAt = null;
        _context.ZaduzenjaOpreme.Add(model);
        await _context.SaveChangesAsync();
        await _context.Entry(model).Reference(x => x.Radnik).LoadAsync();
        await _context.Entry(model).Reference(x => x.RadnaOprema).LoadAsync();
        return CreatedAtAction(nameof(GetById), new { id = model.Id }, ToDTO(model));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ZaduzenjeOpreme model)
    {
        _logger.LogInformation("Updating ZaduzenjeOpreme with id {Id}", id);
        var item = await _context.ZaduzenjaOpreme.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null) return NotFound();
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        item.DatumZaduzenja = model.DatumZaduzenja;
        item.DatumRazduzenja = model.DatumRazduzenja;
        item.RadnikId = model.RadnikId;
        item.RadnaOpremaId = model.RadnaOpremaId;
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Deleting ZaduzenjeOpreme with id {Id}", id);
        var item = await _context.ZaduzenjaOpreme.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null) return NotFound();
        _context.ZaduzenjaOpreme.Remove(item);
        await _context.SaveChangesAsync();
        return Ok();
    }

    private ZaduzenjeOpremeDTO ToDTO(ZaduzenjeOpreme model) => new()
    {
        Id = model.Id,
        DatumZaduzenja = model.DatumZaduzenja,
        DatumRazduzenja = model.DatumRazduzenja,
        Radnik = model.Radnik is null ? null : new RadnikDTO
        {
            Id = model.Radnik.Id,
            Ime = model.Radnik.Ime,
            Prezime = model.Radnik.Prezime,
            RadnoMjesto = model.Radnik.RadnoMjesto,
            Email = model.Radnik.Email,
            Telefon = model.Radnik.Telefon,
            DatumZaposlenja = model.Radnik.DatumZaposlenja,
            Aktivan = model.Radnik.Aktivan
        },
        RadnaOprema = model.RadnaOprema is null ? null : new RadnaOpremaDTO
        {
            Id = model.RadnaOprema.Id,
            Naziv = model.RadnaOprema.Naziv,
            InventarniBroj = model.RadnaOprema.InventarniBroj,
            SerijskiBroj = model.RadnaOprema.SerijskiBroj,
            DatumNabave = model.RadnaOprema.DatumNabave,
            Status = model.RadnaOprema.Status.ToString()
        }
    };
}
