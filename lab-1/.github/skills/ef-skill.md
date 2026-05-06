# EF skill za ovaj projekt

Ova smjernica opisuje kako raditi s Entity Framework Core u aplikaciji **Evidencija odrzavanja radne opreme u zastiti na radu**.

## Dodavanje novog modela

Kada dodajes novu entitetsku klasu, koristi EF Core atribute i veze s drugim entitetima.

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vjezba.Model;

public class NoviEntitet
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Naziv { get; set; } = string.Empty;

    public int RadnaOpremaId { get; set; }

    [ForeignKey(nameof(RadnaOpremaId))]
    public virtual RadnaOprema? RadnaOprema { get; set; }
}
```

Ako je polje obavezno, koristi `[Required]`. Za tekstualna polja koristi `[MaxLength]` prema pravilima projekta, npr. za nazive 100, opise 500, email 200, telefon 50 i adrese 300.

## Dodavanje DbSet u VjezbaDbContext

Svaka nova tablica mora imati svoj `DbSet` u [VjezbaDbContext.cs](../../Vjezba.App/Data/VjezbaDbContext.cs).

```csharp
public DbSet<NoviEntitet> NoviEntiteti { get; set; } = default!;
```

Ako je entitet povezan s drugim tablicama, po potrebi dodaj konfiguraciju relacija u `OnModelCreating`.

```csharp
modelBuilder.Entity<NoviEntitet>()
    .HasOne(x => x.RadnaOprema)
    .WithMany()
    .HasForeignKey(x => x.RadnaOpremaId);
```

## Migracije

Nakon izmjene modela i `DbContext` pokreni novu migraciju.

```powershell
dotnet ef migrations add DodajNoviEntitet
dotnet ef database update
```

Ako se pojavi problem s design-time `DbContext`-om, prvo provjeri da je `VjezbaDbContext` registriran u [Program.cs](../../Vjezba.App/Program.cs).

## Ucitavanje povezanih podataka s Include()

Za citanje povezanih podataka koristi `Include()` i `ThenInclude()` u EF repozitorijima.

```csharp
using Microsoft.EntityFrameworkCore;

public IReadOnlyList<RadnaOprema> GetAll()
{
    return _context.RadnaOprema
        .Include(x => x.Lokacija)
        .Include(x => x.Proizvodac)
        .Include(x => x.Kategorija)
        .ToList();
}
```

Primjer za odrzavanje:

```csharp
public Odrzavanje? GetById(int id)
{
    return _context.Odrzavanja
        .Include(x => x.Izvrsio)
        .Include(x => x.Oprema)
        .FirstOrDefault(x => x.Id == id);
}
```

## Registracija novog EF repozitorija

Novi EF repozitorij registrira se u [Program.cs](../../Vjezba.App/Program.cs) kao scoped servis.

```csharp
builder.Services.AddScoped<EFNoviEntitetRepository>();
```

Primjer iz projekta:

```csharp
builder.Services.AddScoped<EFRadnaOpremaRepository>();
builder.Services.AddScoped<EFOdrzavanjeRepository>();
```

## Primjeri iz projekta

Za postojece modele koristi isti obrazac kao u aplikaciji:

```csharp
modelBuilder.Entity<RadnaOprema>().HasData(
    new RadnaOprema
    {
        Id = 1,
        Naziv = "Busilica-01",
        InventarniBroj = "INV-001",
        SerijskiBroj = "SN-ABC-001",
        DatumNabave = new DateTime(2021, 3, 12),
        Status = StatusOpreme.Neispravna,
        LokacijaId = 1,
        ProizvodacId = 1,
        KategorijaOpremeId = 1
    }
);
```

```csharp
modelBuilder.Entity<Odrzavanje>().HasData(
    new Odrzavanje
    {
        Id = 1,
        Datum = new DateTime(2024, 2, 14),
        Opis = "Servis motora",
        Cijena = 150m,
        IzvrsioId = 1,
        OpremaId = 1,
        Napomena = "Zamijenjen lezaj",
        TrajanjeTicks = TimeSpan.FromHours(2).Ticks
    }
);
```
