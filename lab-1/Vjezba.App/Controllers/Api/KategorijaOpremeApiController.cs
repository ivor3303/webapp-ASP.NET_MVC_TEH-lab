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

    public KategorijaOpremeApiController(VjezbaDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<KategorijaOpremeDTO>>> GetAll([FromQuery] string? q = null)
    {
        var query = _context.KategorijeOpreme.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
        {
            var normalizedQuery = q.Trim();
            query = query.Where(x => x.Naziv.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase));
        }

        var items = await query.Where(x => x.DeletedAt == null).ToListAsync();
        return Ok(items.Select(ToDTO));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<KategorijaOpremeDTO>> GetById(int id)
    {
        var item = await _context.KategorijeOpreme.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
        if (item is null)
        {
            return NotFound();
        }

        return Ok(ToDTO(item));
    }

    [HttpPost]
    public async Task<ActionResult<KategorijaOpremeDTO>> Create([FromBody] KategorijaOpreme model)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        model.DeletedAt = null;
        _context.KategorijeOpreme.Add(model);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = model.Id }, ToDTO(model));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] KategorijaOpreme model)
    {
        var item = await _context.KategorijeOpreme.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        item.Naziv = model.Naziv;
        item.Opis = model.Opis;

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.KategorijeOpreme.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        _context.KategorijeOpreme.Remove(item);
        await _context.SaveChangesAsync();
        return Ok();
    }

    private KategorijaOpremeDTO ToDTO(KategorijaOpreme model)
    {
        return new KategorijaOpremeDTO
        {
            Id = model.Id,
            Naziv = model.Naziv,
            Opis = model.Opis
        };
    }
}