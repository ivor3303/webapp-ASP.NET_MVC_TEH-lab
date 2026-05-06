using Microsoft.EntityFrameworkCore;
using Vjezba.App.Data;
using Vjezba.Model;

namespace Vjezba.App.Repositories.EF;

public class EFZaduzenjeOpremeRepository
{
    private readonly VjezbaDbContext _context;

    public EFZaduzenjeOpremeRepository(VjezbaDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<ZaduzenjeOpreme> GetAll()
    {
        return _context.ZaduzenjaOpreme
            .Include(x => x.Radnik)
            .Include(x => x.RadnaOprema)
            .ToList();
    }

    public ZaduzenjeOpreme? GetById(int id)
    {
        return _context.ZaduzenjaOpreme
            .Include(x => x.Radnik)
            .Include(x => x.RadnaOprema)
            .FirstOrDefault(x => x.Id == id);
    }
}