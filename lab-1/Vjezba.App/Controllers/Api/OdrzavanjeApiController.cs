using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.App.Data;
using Vjezba.App.DTOs;
using Vjezba.Model;

namespace Vjezba.App.Controllers.Api;

[ApiController]
[Route("api/odrzavanje")]
public class OdrzavanjeApiController : ControllerBase
{
    private readonly VjezbaDbContext _context;

    public OdrzavanjeApiController(VjezbaDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OdrzavanjeDTO>>> GetAll([FromQuery] string? q = null)
    {
        var query = _context.Odrzavanja
            .Include(x => x.Oprema)
            .Include(x => x.Izvrsio)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var normalizedQuery = q.Trim();
            query = query.Where(x => x.Opis.Contains(normalizedQuery));
        }

        var items = await query.Where(x => x.DeletedAt == null).ToListAsync();
        return Ok(items.Select(ToDTO));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OdrzavanjeDTO>> GetById(int id)
    {
        var item = await _context.Odrzavanja
            .Include(x => x.Oprema)
            .Include(x => x.Izvrsio)
            .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);

        if (item is null)
        {
            return NotFound();
        }

        return Ok(ToDTO(item));
    }

    [HttpPost]
    public async Task<ActionResult<OdrzavanjeDTO>> Create([FromBody] Odrzavanje model)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        model.DeletedAt = null;
        _context.Odrzavanja.Add(model);
        await _context.SaveChangesAsync();

        await _context.Entry(model).Reference(x => x.Oprema).LoadAsync();
        await _context.Entry(model).Reference(x => x.Izvrsio).LoadAsync();

        return CreatedAtAction(nameof(GetById), new { id = model.Id }, ToDTO(model));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Odrzavanje model)
    {
        var item = await _context.Odrzavanja.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        item.Datum = model.Datum;
        item.Opis = model.Opis;
        item.Cijena = model.Cijena;
        item.Napomena = model.Napomena;
        item.OpremaId = model.OpremaId;
        item.IzvrsioId = model.IzvrsioId;

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Odrzavanja.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        _context.Odrzavanja.Remove(item);
        await _context.SaveChangesAsync();
        return Ok();
    }

    private OdrzavanjeDTO ToDTO(Odrzavanje model)
    {
        return new OdrzavanjeDTO
        {
            Id = model.Id,
            Datum = model.Datum,
            Opis = model.Opis,
            Cijena = model.Cijena,
            Napomena = model.Napomena,
            Oprema = model.Oprema is null ? null : new RadnaOpremaDTO
            {
                Id = model.Oprema.Id,
                Naziv = model.Oprema.Naziv,
                InventarniBroj = model.Oprema.InventarniBroj,
                SerijskiBroj = model.Oprema.SerijskiBroj,
                DatumNabave = model.Oprema.DatumNabave,
                Status = model.Oprema.Status.ToString(),
                Lokacija = model.Oprema.Lokacija is null ? null : new LokacijaDTO
                {
                    Id = model.Oprema.Lokacija.Id,
                    Naziv = model.Oprema.Lokacija.Naziv,
                    Adresa = model.Oprema.Lokacija.Adresa
                },
                Proizvodac = model.Oprema.Proizvodac is null ? null : new ProizvodacDTO
                {
                    Id = model.Oprema.Proizvodac.Id,
                    Naziv = model.Oprema.Proizvodac.Naziv,
                    Drzava = model.Oprema.Proizvodac.Drzava,
                    KontaktEmail = model.Oprema.Proizvodac.KontaktEmail
                },
                Kategorija = model.Oprema.Kategorija is null ? null : new KategorijaOpremeDTO
                {
                    Id = model.Oprema.Kategorija.Id,
                    Naziv = model.Oprema.Kategorija.Naziv,
                    Opis = model.Oprema.Kategorija.Opis
                }
            },
            Izvrsio = model.Izvrsio is null ? null : new RadnikDTO
            {
                Id = model.Izvrsio.Id,
                Ime = model.Izvrsio.Ime,
                Prezime = model.Izvrsio.Prezime,
                RadnoMjesto = model.Izvrsio.RadnoMjesto,
                Email = model.Izvrsio.Email,
                Telefon = model.Izvrsio.Telefon,
                DatumZaposlenja = model.Izvrsio.DatumZaposlenja,
                Aktivan = model.Izvrsio.Aktivan
            }
        };
    }
}