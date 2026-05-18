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
            .Where(x => x.DeletedAt == null)
            .Include(x => x.ServisniZahtjevi)
                .ThenInclude(x => x.Oprema)
            .Include(x => x.Zaduzenja)
                .ThenInclude(x => x.RadnaOprema)
            .ToList();
    }

    public Radnik? GetById(int id)
    {
        return _context.Radnici
            .Where(x => x.DeletedAt == null)
            .Include(x => x.ServisniZahtjevi)
                .ThenInclude(x => x.Oprema)
            .Include(x => x.Zaduzenja)
                .ThenInclude(x => x.RadnaOprema)
            .FirstOrDefault(x => x.Id == id);
    }

    public void Create(Radnik entity)
    {
        _context.Radnici.Add(entity);
        _context.SaveChanges();
    }

    public void Update(Radnik entity)
    {
        _context.Radnici.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var entity = _context.Radnici.FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
        if (entity is null)
        {
            return;
        }

        entity.DeletedAt = DateTime.UtcNow;
        _context.SaveChanges();
    }
}