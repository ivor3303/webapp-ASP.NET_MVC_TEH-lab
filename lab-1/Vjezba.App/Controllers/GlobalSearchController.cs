using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.App.Data;
using Vjezba.App.ViewModels;

namespace Vjezba.App.Controllers;

[Route("search")]
public class GlobalSearchController : Controller
{
    private readonly VjezbaDbContext _context;

    public GlobalSearchController(VjezbaDbContext context)
    {
        _context = context;
    }

    [Route("")]
    [AllowAnonymous]
    public async Task<IActionResult> Index(string q)
    {
        var model = new GlobalSearchViewModel
        {
            Query = q ?? string.Empty
        };

        if (!string.IsNullOrWhiteSpace(q))
        {
            var normalizedQuery = q.Trim().ToLower();

            model.RadnaOprema = await _context.RadnaOprema
                .Where(x => x.DeletedAt == null)
                .Where(x => x.Naziv.ToLower().Contains(normalizedQuery)
                    || x.InventarniBroj.ToLower().Contains(normalizedQuery)
                    || x.SerijskiBroj.ToLower().Contains(normalizedQuery))
                .ToListAsync();

            model.Radnici = await _context.Radnici
                .Where(x => x.DeletedAt == null)
                .Where(x => x.Ime.ToLower().Contains(normalizedQuery)
                    || x.Prezime.ToLower().Contains(normalizedQuery)
                    || x.Email.ToLower().Contains(normalizedQuery)
                    || x.RadnoMjesto.ToLower().Contains(normalizedQuery))
                .ToListAsync();

            model.Lokacije = await _context.Lokacije
                .Where(x => x.DeletedAt == null)
                .Where(x => x.Naziv.ToLower().Contains(normalizedQuery)
                    || x.Adresa.ToLower().Contains(normalizedQuery))
                .ToListAsync();

            model.Proizvodaci = await _context.Proizvodaci
                .Where(x => x.DeletedAt == null)
                .Where(x => x.Naziv.ToLower().Contains(normalizedQuery)
                    || x.Drzava.ToLower().Contains(normalizedQuery)
                    || x.KontaktEmail.ToLower().Contains(normalizedQuery))
                .ToListAsync();

            model.KategorijeOpreme = await _context.KategorijeOpreme
                .Where(x => x.DeletedAt == null)
                .Where(x => x.Naziv.ToLower().Contains(normalizedQuery)
                    || x.Opis.ToLower().Contains(normalizedQuery))
                .ToListAsync();

            model.Odrzavanja = await _context.Odrzavanja
                .Include(x => x.Oprema)
                .Where(x => x.DeletedAt == null)
                .Where(x => x.Opis.ToLower().Contains(normalizedQuery)
                    || x.Napomena.ToLower().Contains(normalizedQuery))
                .ToListAsync();

            model.ServisniZahtjevi = await _context.ServisniZahtjevi
                .Include(x => x.Oprema)
                .Where(x => x.DeletedAt == null)
                .Where(x => x.OpisKvara.ToLower().Contains(normalizedQuery)
                    || x.Komentar.ToLower().Contains(normalizedQuery))
                .ToListAsync();

            model.TotalResults = model.RadnaOprema.Count
                + model.Radnici.Count
                + model.Lokacije.Count
                + model.Proizvodaci.Count
                + model.KategorijeOpreme.Count
                + model.Odrzavanja.Count
                + model.ServisniZahtjevi.Count;
        }

        return View(model);
    }
}
