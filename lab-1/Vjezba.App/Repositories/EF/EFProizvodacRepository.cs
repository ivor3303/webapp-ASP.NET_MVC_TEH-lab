using Vjezba.App.Data;
using Vjezba.Model;

namespace Vjezba.App.Repositories.EF;

public class EFProizvodacRepository
{
    private readonly VjezbaDbContext _context;

    public EFProizvodacRepository(VjezbaDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Proizvodac> GetAll()
    {
        return _context.Proizvodaci.ToList();
    }

    public Proizvodac? GetById(int id)
    {
        return _context.Proizvodaci.FirstOrDefault(x => x.Id == id);
    }
}