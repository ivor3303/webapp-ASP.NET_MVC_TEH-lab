using Microsoft.EntityFrameworkCore;
using Vjezba.App.Data;
using Vjezba.Model;

namespace Vjezba.App.Repositories.EF;

public class EFLokacijaRepository
{
    private readonly VjezbaDbContext _context;

    public EFLokacijaRepository(VjezbaDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Lokacija> GetAll()
    {
        return _context.Lokacije
            .Include(x => x.Oprema)
            .ToList();
    }

    public Lokacija? GetById(int id)
    {
        return _context.Lokacije
            .Include(x => x.Oprema)
            .FirstOrDefault(x => x.Id == id);
    }
}