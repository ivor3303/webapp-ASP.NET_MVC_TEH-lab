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
            .Where(x => x.DeletedAt == null)
            .Include(x => x.Oprema)
            .ToList();
    }

    public ServisniZahtjev? GetById(int id)
    {
        return _context.ServisniZahtjevi
            .Where(x => x.DeletedAt == null)
            .Include(x => x.Oprema)
            .FirstOrDefault(x => x.Id == id);
    }

    public void Create(ServisniZahtjev entity)
    {
        _context.ServisniZahtjevi.Add(entity);
        _context.SaveChanges();
    }

    public void Update(ServisniZahtjev entity)
    {
        _context.ServisniZahtjevi.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var entity = _context.ServisniZahtjevi.FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
        if (entity is null)
        {
            return;
        }

        entity.DeletedAt = DateTime.UtcNow;
        _context.SaveChanges();
    }
}