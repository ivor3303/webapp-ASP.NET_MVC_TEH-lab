using Microsoft.EntityFrameworkCore;
using Vjezba.App.Data;
using Vjezba.App.Repositories.EF;
using Vjezba.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<VjezbaDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("VjezbaDbContext")));
builder.Services.AddScoped<EFRadnaOpremaRepository>();
builder.Services.AddScoped<EFRadnikRepository>();
builder.Services.AddScoped<EFLokacijaRepository>();
builder.Services.AddScoped<EFProizvodacRepository>();
builder.Services.AddScoped<EFKategorijaOpremeRepository>();
builder.Services.AddScoped<EFOdrzavanjeRepository>();
builder.Services.AddScoped<EFServisniZahtjevRepository>();
builder.Services.AddScoped<EFZaduzenjeOpremeRepository>();
builder.Services.AddScoped<EFIzvjestajRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();

    context.Database.Migrate();

    if (context.Lokacije.Count() == 0)
    {
        context.Lokacije.AddRange(
            new Lokacija { Id = 1, Naziv = "Skladiste Zagreb", Adresa = "Industrijska cesta 12, Zagreb" },
            new Lokacija { Id = 2, Naziv = "Servisni centar Split", Adresa = "Put Industrije 8, Split" },
            new Lokacija { Id = 3, Naziv = "Terenski ured Osijek", Adresa = "Zeljeznicka 4, Osijek" });
        context.SaveChanges();

        context.Proizvodaci.AddRange(
            new Proizvodac { Id = 1, Naziv = "Bosch", Drzava = "DE", KontaktEmail = "kontakt@bosch.de" },
            new Proizvodac { Id = 2, Naziv = "Makita", Drzava = "JP", KontaktEmail = "support@makita.jp" },
            new Proizvodac { Id = 3, Naziv = "DeWalt", Drzava = "US", KontaktEmail = "service@dewalt.com" });
        context.SaveChanges();

        context.KategorijeOpreme.AddRange(
            new KategorijaOpreme { Id = 1, Naziv = "Busilice", Opis = "Industrijske busilice za terenski i servisni rad." },
            new KategorijaOpreme { Id = 2, Naziv = "Kompresori", Opis = "Kompresori za napajanje pneumatskih alata." },
            new KategorijaOpreme { Id = 3, Naziv = "Brusilice", Opis = "Rucne i stacionarne brusilice za obradu materijala." });
        context.SaveChanges();

        context.Radnici.AddRange(
            new Radnik
            {
                Id = 1,
                Ime = "Ivan",
                Prezime = "Ivic",
                RadnoMjesto = "Tehnicar",
                Email = "ivan.ivic@tehlab.hr",
                Telefon = "+385911111111",
                DatumZaposlenja = new DateTime(2019, 4, 15),
                Aktivan = true
            },
            new Radnik
            {
                Id = 2,
                Ime = "Ana",
                Prezime = "Anic",
                RadnoMjesto = "Serviser",
                Email = "ana.anic@tehlab.hr",
                Telefon = "+385922222222",
                DatumZaposlenja = new DateTime(2020, 9, 1),
                Aktivan = true
            },
            new Radnik
            {
                Id = 3,
                Ime = "Marko",
                Prezime = "Maric",
                RadnoMjesto = "Koordinator zastite na radu",
                Email = "marko.maric@tehlab.hr",
                Telefon = "+385933333333",
                DatumZaposlenja = new DateTime(2022, 2, 10),
                Aktivan = true
            });
        context.SaveChanges();

        context.RadnaOprema.AddRange(
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
            },
            new RadnaOprema
            {
                Id = 2,
                Naziv = "Kompresor-01",
                InventarniBroj = "INV-002",
                SerijskiBroj = "SN-MK-002",
                DatumNabave = new DateTime(2023, 6, 5),
                Status = StatusOpreme.Ispravna,
                LokacijaId = 2,
                ProizvodacId = 2,
                KategorijaOpremeId = 2
            },
            new RadnaOprema
            {
                Id = 3,
                Naziv = "Brusilica-01",
                InventarniBroj = "INV-003",
                SerijskiBroj = "SN-DW-009",
                DatumNabave = new DateTime(2019, 11, 20),
                Status = StatusOpreme.UServisu,
                LokacijaId = 3,
                ProizvodacId = 3,
                KategorijaOpremeId = 3
            });
        context.SaveChanges();

        context.Odrzavanja.AddRange(
            new Odrzavanje
            {
                Id = 1,
                Datum = new DateTime(2024, 2, 14),
                Opis = "Servis motora",
                Cijena = 150m,
                IzvrsioId = 1,
                TrajanjeTicks = TimeSpan.FromHours(2).Ticks,
                OpremaId = 1,
                Napomena = "Zamijenjen lezaj"
            },
            new Odrzavanje
            {
                Id = 2,
                Datum = new DateTime(2024, 6, 3),
                Opis = "Preventivni pregled ventila",
                Cijena = 80m,
                IzvrsioId = 2,
                TrajanjeTicks = TimeSpan.FromHours(1).Ticks,
                OpremaId = 2,
                Napomena = "Ventili ispravni"
            },
            new Odrzavanje
            {
                Id = 3,
                Datum = new DateTime(2024, 10, 21),
                Opis = "Kalibracija zastitnog stitnika",
                Cijena = 95m,
                IzvrsioId = 1,
                TrajanjeTicks = TimeSpan.FromHours(2).Ticks,
                OpremaId = 3,
                Napomena = "Uredaj vracen u servisni test"
            });
        context.SaveChanges();

        context.ServisniZahtjevi.AddRange(
            new ServisniZahtjev
            {
                Id = 1,
                DatumPrijave = new DateTime(2025, 2, 5),
                OpisKvara = "Povremeni zastoj brusilice",
                Hitno = true,
                OpremaId = 3,
                Komentar = "Prijavio Marko Maric"
            },
            new ServisniZahtjev
            {
                Id = 2,
                DatumPrijave = new DateTime(2025, 3, 18),
                OpisKvara = "Neuobicajen zvuk kompresora",
                Hitno = false,
                OpremaId = 2,
                Komentar = "Planiran redovni pregled"
            });
        context.SaveChanges();

        context.ZaduzenjaOpreme.AddRange(
            new ZaduzenjeOpreme
            {
                Id = 1,
                RadnikId = 1,
                RadnaOpremaId = 1,
                DatumZaduzenja = new DateTime(2024, 7, 1),
                DatumRazduzenja = null
            },
            new ZaduzenjeOpreme
            {
                Id = 2,
                RadnikId = 2,
                RadnaOpremaId = 2,
                DatumZaduzenja = new DateTime(2024, 11, 15),
                DatumRazduzenja = new DateTime(2025, 1, 20)
            });
        context.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

