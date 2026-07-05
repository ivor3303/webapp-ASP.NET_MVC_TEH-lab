using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.App.Data;
using Vjezba.App.DTOs;
using Vjezba.Model;

namespace Vjezba.App.Controllers.Api;

[ApiController]
[Route("api/kategorijaOpreme")]
public class KategorijaOpremeApiController : ControllerBase
{
    private readonly VjezbaDbContext _context;
    private readonly ILogger<KategorijaOpremeApiController> _logger;

    public KategorijaOpremeApiController(VjezbaDbContext context, ILogger<KategorijaOpremeApiController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<KategorijaOpremeDTO>>> GetAll([FromQuery] string? q = null)
    {
        _logger.LogInformation("Fetching all KategorijaOpreme");
        var query = _context.KategorijeOpreme.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
        {
            var normalizedQuery = q.Trim();
            query = query.Where(x => x.Naziv.Contains(normalizedQuery));
        }

        var items = await query.Where(x => x.DeletedAt == null).ToListAsync();
        return Ok(items.Select(ToDTO));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<KategorijaOpremeDTO>> GetById(int id)
    {
        _logger.LogInformation("Fetching KategorijaOpreme with id {Id}", id);
        var item = await _context.KategorijeOpreme.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
        if (item is null)
        {
            _logger.LogWarning("KategorijaOpreme with id {Id} not found", id);
            return NotFound();
        }
        return Ok(ToDTO(item));
    }

    [HttpPost]
    public async Task<ActionResult<KategorijaOpremeDTO>> Create([FromBody] KategorijaOpreme model)
    {
        _logger.LogInformation("Creating new KategorijaOpreme");
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        model.DeletedAt = null;
        _context.KategorijeOpreme.Add(model);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = model.Id }, ToDTO(model));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] KategorijaOpreme model)
    {
        _logger.LogInformation("Updating KategorijaOpreme with id {Id}", id);
        var item = await _context.KategorijeOpreme.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null) return NotFound();
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        item.Naziv = model.Naziv;
        item.Opis = model.Opis;
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Deleting KategorijaOpreme with id {Id}", id);
        var item = await _context.KategorijeOpreme.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null) return NotFound();
        _context.KategorijeOpreme.Remove(item);
        await _context.SaveChangesAsync();
        return Ok();
    }

    private KategorijaOpremeDTO ToDTO(KategorijaOpreme model) => new()
    {
        Id = model.Id,
        Naziv = model.Naziv,
        Opis = model.Opis
    };
}
