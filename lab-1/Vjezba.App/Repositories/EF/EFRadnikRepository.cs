using Microsoft.EntityFrameworkCore;
using Vjezba.App.Data;
using Vjezba.Model;

namespace Vjezba.App.Repositories.EF;

public class EFRadnikRepository
{
    private readonly VjezbaDbContext _context;

    public EFRadnikRepository(VjezbaDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Radnik> GetAll()
    {
        return _context.Radnici
            .Include(x => x.ServisniZahtjevi)
                .ThenInclude(x => x.Oprema)
            .Include(x => x.Zaduzenja)
                .ThenInclude(x => x.RadnaOprema)
            .ToList();
    }

    public Radnik? GetById(int id)
    {
        return _context.Radnici
            .Include(x => x.ServisniZahtjevi)
                .ThenInclude(x => x.Oprema)
            .Include(x => x.Zaduzenja)
                .ThenInclude(x => x.RadnaOprema)
            .FirstOrDefault(x => x.Id == id);
    }
}