using System;
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
        return _context.Proizvodaci
            .Where(x => x.DeletedAt == null)
            .ToList();
    }

    public Proizvodac? GetById(int id)
    {
        return _context.Proizvodaci.FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
    }

    public void Create(Proizvodac entity)
    {
        _context.Proizvodaci.Add(entity);
        _context.SaveChanges();
    }

    public void Update(Proizvodac entity)
    {
        _context.Proizvodaci.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var entity = _context.Proizvodaci.FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
        if (entity is null)
        {
            return;
        }

        entity.DeletedAt = DateTime.UtcNow;
        _context.SaveChanges();
    }
}