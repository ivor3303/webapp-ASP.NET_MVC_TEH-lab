using Microsoft.EntityFrameworkCore;
using Vjezba.App.Data;
using Vjezba.Model;

namespace Vjezba.App.Repositories.EF;

public class EFRadnaOpremaRepository
{
    private readonly VjezbaDbContext _context;

    public EFRadnaOpremaRepository(VjezbaDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<RadnaOprema> GetAll()
    {
        return _context.RadnaOprema
            .Where(x => x.DeletedAt == null)
            .Include(x => x.Lokacija)
            .Include(x => x.Proizvodac)
            .Include(x => x.Kategorija)
            .Include(x => x.Odrzavanja)
                .ThenInclude(x => x.Izvrsio)
            .Include(x => x.Zaduzenja)
                .ThenInclude(x => x.Radnik)
            .ToList();
    }

    public RadnaOprema? GetById(int id)
    {
        return _context.RadnaOprema
            .Where(x => x.DeletedAt == null)
            .Include(x => x.Lokacija)
            .Include(x => x.Proizvodac)
            .Include(x => x.Kategorija)
            .Include(x => x.Odrzavanja)
                .ThenInclude(x => x.Izvrsio)
            .Include(x => x.Zaduzenja)
                .ThenInclude(x => x.Radnik)
            .FirstOrDefault(x => x.Id == id);
    }

    public void Create(RadnaOprema entity)
    {
        _context.RadnaOprema.Add(entity);
        _context.SaveChanges();
    }

    public void Update(RadnaOprema entity)
    {
        _context.RadnaOprema.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var entity = _context.RadnaOprema.FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
        if (entity is null)
        {
            return;
        }

        entity.DeletedAt = DateTime.UtcNow;
        _context.SaveChanges();
    }
}