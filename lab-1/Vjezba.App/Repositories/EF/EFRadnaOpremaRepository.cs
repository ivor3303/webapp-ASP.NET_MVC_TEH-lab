using Microsoft.EntityFrameworkCore;
using Vjezba.App.Data;
using Vjezba.Model;

namespace Vjezba.App.Repositories.EF;

public class EFRadnaOpremaRepository
{
    private readonly VjezbaDbContext _context;

    public EFRadnaOpremaRepository(VjezbaDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<RadnaOprema> GetAll()
    {
        return _context.RadnaOprema
            .Include(x => x.Lokacija)
            .Include(x => x.Proizvodac)
            .Include(x => x.Kategorija)
            .Include(x => x.Odrzavanja)
                .ThenInclude(x => x.Izvrsio)
            .Include(x => x.Zaduzenja)
                .ThenInclude(x => x.Radnik)
            .ToList();
    }

    public RadnaOprema? GetById(int id)
    {
        return _context.RadnaOprema
            .Include(x => x.Lokacija)
            .Include(x => x.Proizvodac)
            .Include(x => x.Kategorija)
            .Include(x => x.Odrzavanja)
                .ThenInclude(x => x.Izvrsio)
            .Include(x => x.Zaduzenja)
                .ThenInclude(x => x.Radnik)
            .FirstOrDefault(x => x.Id == id);
    }
}