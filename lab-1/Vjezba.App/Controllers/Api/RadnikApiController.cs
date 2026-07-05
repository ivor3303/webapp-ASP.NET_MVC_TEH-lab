using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.App.Data;
using Vjezba.App.DTOs;
using Vjezba.Model;

namespace Vjezba.App.Controllers.Api;

[ApiController]
[Route("api/radnik")]
public class RadnikApiController : ControllerBase
{
    private readonly VjezbaDbContext _context;
    private readonly ILogger<RadnikApiController> _logger;

    public RadnikApiController(VjezbaDbContext context, ILogger<RadnikApiController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RadnikDTO>>> GetAll([FromQuery] string? q = null)
    {
        _logger.LogInformation("Fetching all Radnik");
        var query = _context.Radnici.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
        {
            var normalizedQuery = q.Trim();
            query = query.Where(x => x.Ime.Contains(normalizedQuery)
                || x.Prezime.Contains(normalizedQuery)
                || x.Email.Contains(normalizedQuery));
        }

        var items = await query.Where(x => x.DeletedAt == null).ToListAsync();
        return Ok(items.Select(ToDTO));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RadnikDTO>> GetById(int id)
    {
        _logger.LogInformation("Fetching Radnik with id {Id}", id);
        var item = await _context.Radnici.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
        if (item is null)
        {
            _logger.LogWarning("Radnik with id {Id} not found", id);
            return NotFound();
        }

        return Ok(ToDTO(item));
    }

    [HttpPost]
    public async Task<ActionResult<RadnikDTO>> Create([FromBody] Radnik model)
    {
        _logger.LogInformation("Creating new Radnik");
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        model.DeletedAt = null;
        _context.Radnici.Add(model);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = model.Id }, ToDTO(model));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Radnik model)
    {
        _logger.LogInformation("Updating Radnik with id {Id}", id);
        var item = await _context.Radnici.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        item.Ime = model.Ime;
        item.Prezime = model.Prezime;
        item.RadnoMjesto = model.RadnoMjesto;
        item.Email = model.Email;
        item.Telefon = model.Telefon;
        item.Aktivan = model.Aktivan;

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Deleting Radnik with id {Id}", id);
        var item = await _context.Radnici.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        item.Aktivan = false;
        await _context.SaveChangesAsync();
        return Ok();
    }

    private RadnikDTO ToDTO(Radnik model)
    {
        return new RadnikDTO
        {
            Id = model.Id,
            Ime = model.Ime,
            Prezime = model.Prezime,
            RadnoMjesto = model.RadnoMjesto,
            Email = model.Email,
            Telefon = model.Telefon,
            DatumZaposlenja = model.DatumZaposlenja,
            Aktivan = model.Aktivan
        };
    }
}