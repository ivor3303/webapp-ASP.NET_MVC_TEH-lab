using Microsoft.EntityFrameworkCore;
using Vjezba.App.Data;
using Vjezba.Model;

namespace Vjezba.App.Repositories.EF;

public class EFIzvjestajRepository
{
    private readonly VjezbaDbContext _context;

    public EFIzvjestajRepository(VjezbaDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Izvjestaj> GetAll()
    {
        return _context.Izvjestaji
            .Include(x => x.Radnik)
            .ToList();
    }

    public Izvjestaj? GetById(int id)
    {
        return _context.Izvjestaji
            .Include(x => x.Radnik)
            .FirstOrDefault(x => x.Id == id);
    }
}