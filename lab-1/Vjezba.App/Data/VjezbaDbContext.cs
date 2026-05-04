using Microsoft.EntityFrameworkCore;
using Vjezba.Model;

namespace Vjezba.App.Data;

public class VjezbaDbContext : DbContext
{
    public VjezbaDbContext(DbContextOptions<VjezbaDbContext> options)
        : base(options)
    {
    }

    public DbSet<RadnaOprema> RadnaOprema { get; set; } = default!;
    public DbSet<Radnik> Radnici { get; set; } = default!;
    public DbSet<Lokacija> Lokacije { get; set; } = default!;
    public DbSet<Proizvodac> Proizvodaci { get; set; } = default!;
    public DbSet<KategorijaOpreme> KategorijeOpreme { get; set; } = default!;
    public DbSet<Odrzavanje> Odrzavanja { get; set; } = default!;
    public DbSet<ServisniZahtjev> ServisniZahtjevi { get; set; } = default!;
    public DbSet<ZaduzenjeOpreme> ZaduzenjaOpreme { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RadnaOprema>()
            .HasOne(o => o.Lokacija)
            .WithMany(l => l.Oprema)
            .HasForeignKey(o => o.LokacijaId);

        modelBuilder.Entity<RadnaOprema>()
            .HasOne(o => o.Proizvodac)
            .WithMany()
            .HasForeignKey(o => o.ProizvodacId);

        modelBuilder.Entity<RadnaOprema>()
            .HasOne(o => o.Kategorija)
            .WithMany()
            .HasForeignKey(o => o.KategorijaOpremeId);

        modelBuilder.Entity<Odrzavanje>()
            .HasOne(o => o.Oprema)
            .WithMany(op => op.Odrzavanja)
            .HasForeignKey(o => o.OpremaId);

        modelBuilder.Entity<Odrzavanje>()
            .HasOne(o => o.Izvrsio)
            .WithMany()
            .HasForeignKey(o => o.IzvrsioId);

        modelBuilder.Entity<ServisniZahtjev>()
            .HasOne(s => s.Oprema)
            .WithMany()
            .HasForeignKey(s => s.OpremaId);

        modelBuilder.Entity<ZaduzenjeOpreme>()
            .HasOne(z => z.Radnik)
            .WithMany(r => r.Zaduzenja)
            .HasForeignKey(z => z.RadnikId);

        modelBuilder.Entity<ZaduzenjeOpreme>()
            .HasOne(z => z.RadnaOprema)
            .WithMany(o => o.Zaduzenja)
            .HasForeignKey(z => z.RadnaOpremaId);
    }
}