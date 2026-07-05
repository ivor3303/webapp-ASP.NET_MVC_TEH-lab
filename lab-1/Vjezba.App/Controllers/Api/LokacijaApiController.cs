using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.App.Data;
using Vjezba.App.DTOs;
using Vjezba.Model;

namespace Vjezba.App.Controllers.Api;

[ApiController]
[Route("api/lokacija")]
public class LokacijaApiController : ControllerBase
{
    private readonly VjezbaDbContext _context;
    private readonly ILogger<LokacijaApiController> _logger;

    public LokacijaApiController(VjezbaDbContext context, ILogger<LokacijaApiController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LokacijaDTO>>> GetAll([FromQuery] string? q = null)
    {
        _logger.LogInformation("Fetching all Lokacija");
        var query = _context.Lokacije.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
        {
            var normalizedQuery = q.Trim();
            query = query.Where(x => x.Naziv.Contains(normalizedQuery)
                || x.Adresa.Contains(normalizedQuery));
        }

        var items = await query.Where(x => x.DeletedAt == null).ToListAsync();
        return Ok(items.Select(ToDTO));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LokacijaDTO>> GetById(int id)
    {
        _logger.LogInformation("Fetching Lokacija with id {Id}", id);
        var item = await _context.Lokacije.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
        if (item is null)
        {
            _logger.LogWarning("Lokacija with id {Id} not found", id);
            return NotFound();
        }

        return Ok(ToDTO(item));
    }

    [HttpPost]
    public async Task<ActionResult<LokacijaDTO>> Create([FromBody] Lokacija model)
    {
        _logger.LogInformation("Creating new Lokacija");
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        model.DeletedAt = null;
        _context.Lokacije.Add(model);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = model.Id }, ToDTO(model));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Lokacija model)
    {
        _logger.LogInformation("Updating Lokacija with id {Id}", id);
        var item = await _context.Lokacije.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        item.Naziv = model.Naziv;
        item.Adresa = model.Adresa;

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Deleting Lokacija with id {Id}", id);
        var item = await _context.Lokacije.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
        if (item is null)
        {
            return NotFound();
        }

        item.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return Ok();
    }

    private LokacijaDTO ToDTO(Lokacija model)
    {
        return new LokacijaDTO
        {
            Id = model.Id,
            Naziv = model.Naziv,
            Adresa = model.Adresa
        };
    }
}