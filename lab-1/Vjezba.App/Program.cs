using Microsoft.EntityFrameworkCore;
using Vjezba.App.Data;
using Vjezba.App.Repositories.EF;

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

var app = builder.Build();

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

