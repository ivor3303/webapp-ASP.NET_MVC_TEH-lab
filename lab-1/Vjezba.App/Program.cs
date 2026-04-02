using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vjezba.Model;

Console.WriteLine("Primjer rada s modelom Vjezba.Model\n");

var radnik1 = new Radnik { Id = 1, Ime = "Ivan", Prezime = "Ivic", RadnoMjesto = "Tehničar" };
var radnik2 = new Radnik { Id = 2, Ime = "Ana", Prezime = "Anic", RadnoMjesto = "Serviser" };

var loc1 = new Lokacija { Id = 1, Naziv = "Skladište A", Adresa = "Ulica 1" };
var loc2 = new Lokacija { Id = 2, Naziv = "Skladište B", Adresa = "Ulica 2" };

var prod1 = new Proizvodac { Id = 1, Naziv = "Bosch", Drzava = "DE", KontaktEmail = "kontakt@bosch.de" };
var prod2 = new Proizvodac { Id = 2, Naziv = "Makita", Drzava = "JP", KontaktEmail = "kontakt@makita.jp" };

var kat1 = new KategorijaOpreme { Id = 1, Naziv = "Bušilice", Opis = "Industrijske bušilice" };
var kat2 = new KategorijaOpreme { Id = 2, Naziv = "Kompresori", Opis = "Kompresori za zrak" };
var kat3 = new KategorijaOpreme { Id = 3, Naziv = "Brusilice", Opis = "Ručne i stacionarne brusilice" };

var oprema = new List<RadnaOprema>
{
	new RadnaOprema
	{
		Id = 1,
		Naziv = "Bušilica-01",
		InventarniBroj = "INV-001",
		SerijskiBroj = "SN-ABC-001",
		DatumNabave = DateTime.Now.AddYears(-4),
		Status = StatusOpreme.Neispravna,
		Lokacija = loc1,
		Proizvodac = prod1,
		Kategorija = kat1,
		Odrzavanja = new List<Odrzavanje>
		{
			new Odrzavanje { Id = 1, Datum = DateTime.Now.AddYears(-2), Opis = "Servis motora", Cijena = 150m, Izvrsio = radnik1 },
			new Odrzavanje { Id = 2, Datum = DateTime.Now.AddYears(-1), Opis = "Zamjena vretena", Cijena = 80m, Izvrsio = radnik2 }
		}
	},
	new RadnaOprema
	{
		Id = 2,
		Naziv = "Kompresor-01",
		InventarniBroj = "INV-002",
		SerijskiBroj = "SN-PR-001",
		DatumNabave = DateTime.Now.AddYears(-1),
		Status = StatusOpreme.Ispravna,
		Lokacija = loc2,
		Proizvodac = prod2,
		Kategorija = kat2,
		Odrzavanja = new List<Odrzavanje>
		{
			new Odrzavanje { Id = 3, Datum = DateTime.Now.AddMonths(-10), Opis = "Pregled sigurnosnih ventila", Cijena = 60m, Izvrsio = radnik2 }
		}
	},
	new RadnaOprema
	{
		Id = 3,
		Naziv = "Brusilica-01",
		InventarniBroj = "INV-003",
		SerijskiBroj = "SN-ABC-002",
		DatumNabave = DateTime.Now.AddYears(-5),
		Status = StatusOpreme.Otpisana,
		Lokacija = loc1,
		Proizvodac = prod1,
		Kategorija = kat3,
		Odrzavanja = new List<Odrzavanje>()
	}
};

var zaduzenja = new List<ZaduzenjeOpreme>
{
    new ZaduzenjeOpreme
    {
        Id = 1,
        Radnik = radnik1,
        RadnaOprema = oprema[0],
        DatumZaduzenja = DateTime.Now.AddMonths(-6),
        DatumRazduzenja = null
    },
    new ZaduzenjeOpreme
    {
        Id = 2,
        Radnik = radnik2,
        RadnaOprema = oprema[1],
        DatumZaduzenja = DateTime.Now.AddMonths(-3),
        DatumRazduzenja = DateTime.Now.AddMonths(-1)
    }
};

// LINQ upiti
var neispravna = oprema.Where(o => o.Status == StatusOpreme.Neispravna).ToList();
var byProd1 = oprema.Where(o => o.Proizvodac?.Naziv == "Bosch").ToList();
var olderThan3Years = oprema.Where(o => (DateTime.Now - o.DatumNabave).TotalDays > 365 * 3).ToList();
var maintenanceCounts = oprema.Select(o => new { o.Naziv, Count = o.Odrzavanja?.Count ?? 0 }).ToList();
var sortedByDate = oprema.OrderBy(o => o.DatumNabave).ToList();
var findOne = oprema.FirstOrDefault(o => o.InventarniBroj == "INV-002");
var avgMaintenancePrice = oprema.SelectMany(o => o.Odrzavanja ?? new List<Odrzavanje>()).Select(x => x.Cijena).DefaultIfEmpty(0).Average();
var hasExpensiveMaintenance = oprema.Any(o => o.Odrzavanja.Any(odr => odr.Cijena > 100));
Console.WriteLine($"\nPostoji li oprema s održavanjem skupljim od 100: {(hasExpensiveMaintenance ? "Da" : "Ne")}");

// Ispis rezultata
Console.WriteLine("Neispravna oprema:");
foreach (var o in neispravna) Console.WriteLine($" - {o.Naziv} ({o.InventarniBroj})");

Console.WriteLine("\nOprema proizvođača Bosch:");
foreach (var o in byProd1) Console.WriteLine($" - {o.Naziv} ({o.Proizvodac.Naziv})");

Console.WriteLine("\nOprema starija od 3 godine:");
foreach (var o in olderThan3Years) Console.WriteLine($" - {o.Naziv} (nabavljeno: {o.DatumNabave:d})");

Console.WriteLine("\nBroj održavanja po opremi:");
foreach (var m in maintenanceCounts) Console.WriteLine($" - {m.Naziv}: {m.Count}");

Console.WriteLine("\nOprema sortirana po datumu nabave:");
foreach (var o in sortedByDate) Console.WriteLine($" - {o.Naziv}: {o.DatumNabave:d}");

Console.WriteLine("\nPronađeni element (INV-002):");
Console.WriteLine(findOne != null ? $" - {findOne.Naziv} ({findOne.InventarniBroj})" : " - Nije pronađen");

Console.WriteLine($"\nProsječna cijena održavanja: {avgMaintenancePrice:C}");

Console.WriteLine("\nZaduzenja opreme:");
foreach (var z in zaduzenja)
{
    Console.WriteLine($" - Radnik: {z.Radnik.Ime} {z.Radnik.Prezime}, Oprema: {z.RadnaOprema.Naziv}, Datum zaduzenja: {z.DatumZaduzenja:d}, Datum razduzenja: {(z.DatumRazduzenja.HasValue ? z.DatumRazduzenja.Value.ToString("d") : "N/A")}");
}

// Poziv async metode
await SimulateAsyncWork();

Console.WriteLine("\nGotovo.");

static async Task SimulateAsyncWork()
{
	Console.WriteLine("\nPokreće se async zadatak (simulacija)");
	await Task.Delay(500);
	Console.WriteLine("Async zadatak dovršen.");
}

static async Task LoadMaintenanceDataAsync(List<RadnaOprema> oprema)
{
    Console.WriteLine("\nUčitavanje podataka o održavanju...");
    await Task.Delay(1000); // Simulacija kašnjenja
    foreach (var o in oprema)
    {
        Console.WriteLine($" - Oprema: {o.Naziv}, Broj održavanja: {o.Odrzavanja.Count}");
    }
    Console.WriteLine("Podaci o održavanju učitani.");
}

// Poziv metode u Main
await LoadMaintenanceDataAsync(oprema);

