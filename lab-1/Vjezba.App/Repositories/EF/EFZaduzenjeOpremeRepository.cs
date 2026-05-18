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
            .Where(x => x.DeletedAt == null)
            .Include(x => x.Radnik)
            .Include(x => x.RadnaOprema)
            .ToList();
    }

    public ZaduzenjeOpreme? GetById(int id)
    {
        return _context.ZaduzenjaOpreme
            .Where(x => x.DeletedAt == null)
            .Include(x => x.Radnik)
            .Include(x => x.RadnaOprema)
            .FirstOrDefault(x => x.Id == id);
    }

    public void Create(ZaduzenjeOpreme entity)
    {
        _context.ZaduzenjaOpreme.Add(entity);
        _context.SaveChanges();
    }

    public void Update(ZaduzenjeOpreme entity)
    {
        _context.ZaduzenjaOpreme.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var entity = _context.ZaduzenjaOpreme.FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
        if (entity is null)
        {
            return;
        }

        entity.DeletedAt = DateTime.UtcNow;
        _context.SaveChanges();
    }
}