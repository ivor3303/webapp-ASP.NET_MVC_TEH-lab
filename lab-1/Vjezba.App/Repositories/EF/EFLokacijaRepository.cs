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
            .Where(x => x.DeletedAt == null)
            .Include(x => x.Oprema)
            .ToList();
    }

    public Lokacija? GetById(int id)
    {
        return _context.Lokacije
            .Where(x => x.DeletedAt == null)
            .Include(x => x.Oprema)
            .FirstOrDefault(x => x.Id == id);
    }

    public void Create(Lokacija entity)
    {
        _context.Lokacije.Add(entity);
        _context.SaveChanges();
    }

    public void Update(Lokacija entity)
    {
        _context.Lokacije.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var entity = _context.Lokacije.FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
        if (entity is null)
        {
            return;
        }

        entity.DeletedAt = DateTime.UtcNow;
        _context.SaveChanges();
    }
}