using Vjezba.Model;

namespace Vjezba.App.Repositories;

public static class MockDataStore
{
    public static IReadOnlyList<Radnik> Radnici { get; }
    public static IReadOnlyList<Lokacija> Lokacije { get; }
    public static IReadOnlyList<Proizvodac> Proizvodaci { get; }
    public static IReadOnlyList<KategorijaOpreme> Kategorije { get; }
    public static IReadOnlyList<RadnaOprema> RadnaOprema { get; }
    public static IReadOnlyList<Odrzavanje> Odrzavanja { get; }
    public static IReadOnlyList<ServisniZahtjev> ServisniZahtjevi { get; }
    public static IReadOnlyList<ZaduzenjeOpreme> Zaduzenja { get; }

    static MockDataStore()
    {
        var radnici = new List<Radnik>
        {
            new() { Id = 1, Ime = "Ivan", Prezime = "Ivic", RadnoMjesto = "Tehnicar", Email = "ivan.ivic@tehlab.hr", Telefon = "+385911111111", DatumZaposlenja = DateTime.Today.AddYears(-6), Aktivan = true },
            new() { Id = 2, Ime = "Ana", Prezime = "Anic", RadnoMjesto = "Serviser", Email = "ana.anic@tehlab.hr", Telefon = "+385922222222", DatumZaposlenja = DateTime.Today.AddYears(-4), Aktivan = true },
            new() { Id = 3, Ime = "Marko", Prezime = "Maric", RadnoMjesto = "Koordinator zastite na radu", Email = "marko.maric@tehlab.hr", Telefon = "+385933333333", DatumZaposlenja = DateTime.Today.AddYears(-2), Aktivan = true }
        };

        var lokacije = new List<Lokacija>
        {
            new() { Id = 1, Naziv = "Skladiste A", Adresa = "Ulica 1" },
            new() { Id = 2, Naziv = "Skladiste B", Adresa = "Ulica 2" },
            new() { Id = 3, Naziv = "Terenski kontejner", Adresa = "Industrijska zona 5" }
        };

        var proizvodaci = new List<Proizvodac>
        {
            new() { Id = 1, Naziv = "Bosch", Drzava = "DE", KontaktEmail = "kontakt@bosch.de" },
            new() { Id = 2, Naziv = "Makita", Drzava = "JP", KontaktEmail = "kontakt@makita.jp" },
            new() { Id = 3, Naziv = "DeWalt", Drzava = "US", KontaktEmail = "support@dewalt.com" }
        };

        var kategorije = new List<KategorijaOpreme>
        {
            new() { Id = 1, Naziv = "Busilice", Opis = "Industrijske busilice" },
            new() { Id = 2, Naziv = "Kompresori", Opis = "Kompresori za zrak" },
            new() { Id = 3, Naziv = "Brusilice", Opis = "Rucne i stacionarne brusilice" }
        };

        var radnaOprema = new List<RadnaOprema>
        {
            new()
            {
                Id = 1,
                Naziv = "Busilica-01",
                InventarniBroj = "INV-001",
                SerijskiBroj = "SN-ABC-001",
                DatumNabave = DateTime.Today.AddYears(-4),
                Status = StatusOpreme.Neispravna,
                Lokacija = lokacije[0],
                Proizvodac = proizvodaci[0],
                Kategorija = kategorije[0]
            },
            new()
            {
                Id = 2,
                Naziv = "Kompresor-01",
                InventarniBroj = "INV-002",
                SerijskiBroj = "SN-PR-001",
                DatumNabave = DateTime.Today.AddYears(-1),
                Status = StatusOpreme.Ispravna,
                Lokacija = lokacije[1],
                Proizvodac = proizvodaci[1],
                Kategorija = kategorije[1]
            },
            new()
            {
                Id = 3,
                Naziv = "Brusilica-01",
                InventarniBroj = "INV-003",
                SerijskiBroj = "SN-DW-009",
                DatumNabave = DateTime.Today.AddYears(-5),
                Status = StatusOpreme.UServisu,
                Lokacija = lokacije[2],
                Proizvodac = proizvodaci[2],
                Kategorija = kategorije[2]
            }
        };

        var odrzavanja = new List<Odrzavanje>
        {
            new() { Id = 1, Datum = DateTime.Today.AddYears(-2), Opis = "Servis motora", Cijena = 150m, Izvrsio = radnici[0], Oprema = radnaOprema[0], Napomena = "Zamijenjen lezaj", Trajanje = TimeSpan.FromHours(2) },
            new() { Id = 2, Datum = DateTime.Today.AddYears(-1), Opis = "Zamjena vretena", Cijena = 80m, Izvrsio = radnici[1], Oprema = radnaOprema[0], Napomena = "Preventivni zahvat", Trajanje = TimeSpan.FromHours(1.5) },
            new() { Id = 3, Datum = DateTime.Today.AddMonths(-10), Opis = "Pregled sigurnosnih ventila", Cijena = 60m, Izvrsio = radnici[1], Oprema = radnaOprema[1], Napomena = "Svi ventili ispravni", Trajanje = TimeSpan.FromHours(1) },
            new() { Id = 4, Datum = DateTime.Today.AddMonths(-2), Opis = "Kalibracija zastitnog stitnika", Cijena = 95m, Izvrsio = radnici[0], Oprema = radnaOprema[2], Napomena = "Pusteno u servisni test", Trajanje = TimeSpan.FromHours(2.5) }
        };

        var servisniZahtjevi = new List<ServisniZahtjev>
        {
            new() { Id = 1, DatumPrijave = DateTime.Today.AddMonths(-2), OpisKvara = "Povremeni zastoj brusilice", Hitno = true, Oprema = radnaOprema[2], Komentar = "Prijavio Marko Maric" },
            new() { Id = 2, DatumPrijave = DateTime.Today.AddDays(-20), OpisKvara = "Neuobicajen zvuk kompresora", Hitno = false, Oprema = radnaOprema[1], Komentar = "Planiran redovni pregled" }
        };

        var zaduzenja = new List<ZaduzenjeOpreme>
        {
            new() { Id = 1, Radnik = radnici[0], RadnaOprema = radnaOprema[0], DatumZaduzenja = DateTime.Today.AddMonths(-6), DatumRazduzenja = null },
            new() { Id = 2, Radnik = radnici[1], RadnaOprema = radnaOprema[1], DatumZaduzenja = DateTime.Today.AddMonths(-3), DatumRazduzenja = DateTime.Today.AddMonths(-1) },
            new() { Id = 3, Radnik = radnici[2], RadnaOprema = radnaOprema[2], DatumZaduzenja = DateTime.Today.AddMonths(-2), DatumRazduzenja = null }
        };

        foreach (var lokacija in lokacije)
        {
            lokacija.Oprema = radnaOprema.Where(o => o.Lokacija.Id == lokacija.Id).ToList();
        }

        foreach (var oprema in radnaOprema)
        {
            oprema.Odrzavanja = odrzavanja.Where(o => o.Oprema.Id == oprema.Id).ToList();
            oprema.Zaduzenja = zaduzenja.Where(z => z.RadnaOprema.Id == oprema.Id).ToList();
        }

        foreach (var radnik in radnici)
        {
            radnik.ServisniZahtjevi = servisniZahtjevi
                .Where(sz => sz.Komentar.Contains(radnik.Ime, StringComparison.OrdinalIgnoreCase))
                .ToList();
            radnik.Zaduzenja = zaduzenja.Where(z => z.Radnik.Id == radnik.Id).ToList();
        }

        Radnici = radnici;
        Lokacije = lokacije;
        Proizvodaci = proizvodaci;
        Kategorije = kategorije;
        RadnaOprema = radnaOprema;
        Odrzavanja = odrzavanja;
        ServisniZahtjevi = servisniZahtjevi;
        Zaduzenja = zaduzenja;
    }
}
