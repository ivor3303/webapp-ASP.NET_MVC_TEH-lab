using Vjezba.App.Data;
using Vjezba.Model;

namespace Vjezba.App.Repositories.EF;

public class EFKategorijaOpremeRepository
{
    private readonly VjezbaDbContext _context;

    public EFKategorijaOpremeRepository(VjezbaDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<KategorijaOpreme> GetAll()
    {
        return _context.KategorijeOpreme.ToList();
    }

    public KategorijaOpreme? GetById(int id)
    {
        return _context.KategorijeOpreme.FirstOrDefault(x => x.Id == id);
    }
}