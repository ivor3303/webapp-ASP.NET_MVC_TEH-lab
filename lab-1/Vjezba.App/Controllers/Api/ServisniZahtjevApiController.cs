using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.App.Data;
using Vjezba.App.DTOs;
using Vjezba.Model;

namespace Vjezba.App.Controllers.Api;

[ApiController]
[Route("api/servisniZahtjev")]
public class ServisniZahtjevApiController : ControllerBase
{
    private readonly VjezbaDbContext _context;
    private readonly ILogger<ServisniZahtjevApiController> _logger;

    public ServisniZahtjevApiController(VjezbaDbContext context, ILogger<ServisniZahtjevApiController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServisniZahtjevDTO>>> GetAll([FromQuery] string? q = null)
    {
        _logger.LogInformation("Fetching all ServisniZahtjev");
        var query = _context.ServisniZahtjevi.Include(x => x.Oprema).AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
        {
            var normalizedQuery = q.Trim();
            query = query.Where(x => x.OpisKvara.Contains(normalizedQuery) || x.Komentar.Contains(normalizedQuery));
        }

        var items = await query.Where(x => x.DeletedAt == null).ToListAsync();
        return Ok(items.Select(ToDTO));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServisniZahtjevDTO>> GetById(int id)
    {
        _logger.LogInformation("Fetching ServisniZahtjev with id {Id}", id);
        var item = await _context.ServisniZahtjevi.Include(x => x.Oprema).FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
        if (item is null)
        {
            _logger.LogWarning("ServisniZahtjev with id {Id} not found", id);
            return NotFound();
        }
        return Ok(ToDTO(item));
    }

    [HttpPost]
    public async Task<ActionResult<ServisniZahtjevDTO>> Create([FromBody] ServisniZahtjev model)
    {
        _logger.LogInformation("Creating new ServisniZahtjev");
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        model.DeletedAt = null;
        _context.ServisniZahtjevi.Add(model);
        await _context.SaveChangesAsync();
        await _context.Entry(model).Reference(x => x.Oprema).LoadAsync();
        return CreatedAtAction(nameof(GetById), new { id = model.Id }, ToDTO(model));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ServisniZahtjev model)
    {
        _logger.LogInformation("Updating ServisniZahtjev with id {Id}", id);
        var item = await _context.ServisniZahtjevi.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null) return NotFound();
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        item.DatumPrijave = model.DatumPrijave;
        item.OpisKvara = model.OpisKvara;
        item.Hitno = model.Hitno;
        item.Komentar = model.Komentar;
        item.OpremaId = model.OpremaId;
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Deleting ServisniZahtjev with id {Id}", id);
        var item = await _context.ServisniZahtjevi.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null) return NotFound();
        _context.ServisniZahtjevi.Remove(item);
        await _context.SaveChangesAsync();
        return Ok();
    }

    private ServisniZahtjevDTO ToDTO(ServisniZahtjev model) => new()
    {
        Id = model.Id,
        DatumPrijave = model.DatumPrijave,
        OpisKvara = model.OpisKvara,
        Hitno = model.Hitno,
        Komentar = model.Komentar,
        Oprema = model.Oprema is null ? null : new RadnaOpremaDTO
        {
            Id = model.Oprema.Id,
            Naziv = model.Oprema.Naziv,
            InventarniBroj = model.Oprema.InventarniBroj,
            SerijskiBroj = model.Oprema.SerijskiBroj,
            DatumNabave = model.Oprema.DatumNabave,
            Status = model.Oprema.Status.ToString()
        }
    };
}
