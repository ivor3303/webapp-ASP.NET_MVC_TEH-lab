using System;
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
        return _context.KategorijeOpreme
            .Where(x => x.DeletedAt == null)
            .ToList();
    }

    public KategorijaOpreme? GetById(int id)
    {
        return _context.KategorijeOpreme.FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
    }

    public void Create(KategorijaOpreme entity)
    {
        _context.KategorijeOpreme.Add(entity);
        _context.SaveChanges();
    }

    public void Update(KategorijaOpreme entity)
    {
        _context.KategorijeOpreme.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var entity = _context.KategorijeOpreme.FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
        if (entity is null)
        {
            return;
        }

        entity.DeletedAt = DateTime.UtcNow;
        _context.SaveChanges();
    }
}