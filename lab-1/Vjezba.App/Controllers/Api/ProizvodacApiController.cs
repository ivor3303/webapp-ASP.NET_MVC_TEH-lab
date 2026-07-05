using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.App.Data;
using Vjezba.App.DTOs;
using Vjezba.Model;

namespace Vjezba.App.Controllers.Api;

[ApiController]
[Route("api/proizvodac")]
public class ProizvodacApiController : ControllerBase
{
    private readonly VjezbaDbContext _context;
    private readonly ILogger<ProizvodacApiController> _logger;

    public ProizvodacApiController(VjezbaDbContext context, ILogger<ProizvodacApiController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProizvodacDTO>>> GetAll([FromQuery] string? q = null)
    {
        _logger.LogInformation("Fetching all Proizvodac");
        var query = _context.Proizvodaci.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
        {
            var normalizedQuery = q.Trim();
            query = query.Where(x => x.Naziv.Contains(normalizedQuery) || x.Drzava.Contains(normalizedQuery));
        }

        var items = await query.Where(x => x.DeletedAt == null).ToListAsync();
        return Ok(items.Select(ToDTO));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProizvodacDTO>> GetById(int id)
    {
        _logger.LogInformation("Fetching Proizvodac with id {Id}", id);
        var item = await _context.Proizvodaci.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
        if (item is null)
        {
            _logger.LogWarning("Proizvodac with id {Id} not found", id);
            return NotFound();
        }
        return Ok(ToDTO(item));
    }

    [HttpPost]
    public async Task<ActionResult<ProizvodacDTO>> Create([FromBody] Proizvodac model)
    {
        _logger.LogInformation("Creating new Proizvodac");
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        model.DeletedAt = null;
        _context.Proizvodaci.Add(model);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = model.Id }, ToDTO(model));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Proizvodac model)
    {
        _logger.LogInformation("Updating Proizvodac with id {Id}", id);
        var item = await _context.Proizvodaci.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null) return NotFound();
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        item.Naziv = model.Naziv;
        item.Drzava = model.Drzava;
        item.KontaktEmail = model.KontaktEmail;
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Deleting Proizvodac with id {Id}", id);
        var item = await _context.Proizvodaci.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null) return NotFound();
        _context.Proizvodaci.Remove(item);
        await _context.SaveChangesAsync();
        return Ok();
    }

    private ProizvodacDTO ToDTO(Proizvodac model) => new()
    {
        Id = model.Id,
        Naziv = model.Naziv,
        Drzava = model.Drzava,
        KontaktEmail = model.KontaktEmail
    };
}
