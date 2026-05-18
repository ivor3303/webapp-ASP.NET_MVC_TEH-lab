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
            .Where(x => x.DeletedAt == null)
            .Include(x => x.Izvrsio)
            .Include(x => x.Oprema)
            .ToList();
    }

    public Odrzavanje? GetById(int id)
    {
        return _context.Odrzavanja
            .Where(x => x.DeletedAt == null)
            .Include(x => x.Izvrsio)
            .Include(x => x.Oprema)
            .FirstOrDefault(x => x.Id == id);
    }

    public void Create(Odrzavanje entity)
    {
        _context.Odrzavanja.Add(entity);
        _context.SaveChanges();
    }

    public void Update(Odrzavanje entity)
    {
        _context.Odrzavanja.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var entity = _context.Odrzavanja.FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
        if (entity is null)
        {
            return;
        }

        entity.DeletedAt = DateTime.UtcNow;
        _context.SaveChanges();
    }
}