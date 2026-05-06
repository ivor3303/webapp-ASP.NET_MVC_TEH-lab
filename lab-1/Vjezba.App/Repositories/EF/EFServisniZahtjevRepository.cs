using Microsoft.EntityFrameworkCore;
using Vjezba.App.Data;
using Vjezba.Model;

namespace Vjezba.App.Repositories.EF;

public class EFServisniZahtjevRepository
{
    private readonly VjezbaDbContext _context;

    public EFServisniZahtjevRepository(VjezbaDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<ServisniZahtjev> GetAll()
    {
        return _context.ServisniZahtjevi
            .Include(x => x.Oprema)
            .ToList();
    }

    public ServisniZahtjev? GetById(int id)
    {
        return _context.ServisniZahtjevi
            .Include(x => x.Oprema)
            .FirstOrDefault(x => x.Id == id);
    }
}