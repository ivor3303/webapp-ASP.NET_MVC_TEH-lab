using Vjezba.App.Data;
using Vjezba.Model;

namespace Vjezba.Tests;

public static class ApiTestSeeders
{
    public static void ResetDatabase(VjezbaDbContext db)
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
    }

    public static Lokacija SeedLokacija(VjezbaDbContext db, string suffix = "1")
    {
        var entity = new Lokacija
        {
            Naziv = $"Lokacija {suffix}",
            Adresa = $"Ulica {suffix} 1"
        };

        db.Lokacije.Add(entity);
        db.SaveChanges();
        return entity;
    }

    public static Proizvodac SeedProizvodac(VjezbaDbContext db, string suffix = "1")
    {
        var entity = new Proizvodac
        {
            Naziv = $"Proizvodac {suffix}",
            Drzava = "HR",
            KontaktEmail = $"proizvodac{suffix}@example.com"
        };

        db.Proizvodaci.Add(entity);
        db.SaveChanges();
        return entity;
    }

    public static KategorijaOpreme SeedKategorijaOpreme(VjezbaDbContext db, string suffix = "1")
    {
        var entity = new KategorijaOpreme
        {
            Naziv = $"Kategorija {suffix}",
            Opis = $"Opis {suffix}"
        };

        db.KategorijeOpreme.Add(entity);
        db.SaveChanges();
        return entity;
    }

    public static RadnaOprema SeedRadnaOprema(VjezbaDbContext db, string suffix = "1")
    {
        var lokacija = SeedLokacija(db, suffix);
        var proizvodac = SeedProizvodac(db, suffix);
        var kategorija = SeedKategorijaOpreme(db, suffix);

        var entity = new RadnaOprema
        {
            Naziv = $"Oprema {suffix}",
            InventarniBroj = $"INV-{suffix}",
            SerijskiBroj = $"SN-{suffix}",
            DatumNabave = new DateTime(2024, 1, 15),
            Status = StatusOpreme.Ispravna,
            LokacijaId = lokacija.Id,
            ProizvodacId = proizvodac.Id,
            KategorijaOpremeId = kategorija.Id
        };

        db.RadnaOprema.Add(entity);
        db.SaveChanges();
        return entity;
    }

    public static Radnik SeedRadnik(VjezbaDbContext db, string suffix = "1")
    {
        var entity = new Radnik
        {
            Ime = $"Ime {suffix}",
            Prezime = $"Prezime {suffix}",
            RadnoMjesto = "Tehnicar",
            Email = $"radnik{suffix}@example.com",
            Telefon = $"091{suffix.PadLeft(7, '0')}",
            DatumZaposlenja = new DateTime(2023, 5, 10),
            Aktivan = true
        };

        db.Radnici.Add(entity);
        db.SaveChanges();
        return entity;
    }

    public static Odrzavanje SeedOdrzavanje(VjezbaDbContext db, string suffix = "1")
    {
        var radnik = SeedRadnik(db, suffix);
        var oprema = SeedRadnaOprema(db, suffix);

        var entity = new Odrzavanje
        {
            Datum = new DateTime(2024, 6, 1),
            Opis = $"Odrzavanje {suffix}",
            Cijena = 50m,
            Napomena = $"Napomena {suffix}",
            OpremaId = oprema.Id,
            IzvrsioId = radnik.Id,
            TrajanjeTicks = TimeSpan.FromHours(1).Ticks
        };

        db.Odrzavanja.Add(entity);
        db.SaveChanges();
        return entity;
    }
}