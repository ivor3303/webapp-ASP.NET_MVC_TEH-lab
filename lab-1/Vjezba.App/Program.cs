using Vjezba.App.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<RadnaOpremaRepository>();
builder.Services.AddSingleton<ProizvodacRepository>();
builder.Services.AddSingleton<KategorijaOpremeRepository>();
builder.Services.AddSingleton<LokacijaRepository>();
builder.Services.AddSingleton<OdrzavanjeRepository>();
builder.Services.AddSingleton<RadnikRepository>();
builder.Services.AddSingleton<ServisniZahtjevRepository>();
builder.Services.AddSingleton<ZaduzenjeOpremeRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

