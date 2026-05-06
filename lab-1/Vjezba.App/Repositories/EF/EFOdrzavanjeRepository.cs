using Microsoft.EntityFrameworkCore;
using Vjezba.App.Data;
using Vjezba.Model;

namespace Vjezba.App.Repositories.EF;

public class EFOdrzavanjeRepository
{
    private readonly VjezbaDbContext _context;

    public EFOdrzavanjeRepository(VjezbaDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Odrzavanje> GetAll()
    {
        return _context.Odrzavanja
            .Include(x => x.Izvrsio)
            .Include(x => x.Oprema)
            .ToList();
    }

    public Odrzavanje? GetById(int id)
    {
        return _context.Odrzavanja
            .Include(x => x.Izvrsio)
            .Include(x => x.Oprema)
            .FirstOrDefault(x => x.Id == id);
    }
}