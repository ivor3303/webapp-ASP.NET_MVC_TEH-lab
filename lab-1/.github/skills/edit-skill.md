# Skill za izradu Edit/Create forme

Ova smjernica opisuje kako izraditi formu za unos i izmjenu podataka u aplikaciji **Evidencija odrzavanja radne opreme u zastiti na radu**.

## 1. GET i POST akcije u controlleru

Za unos ili izmjenu podataka koristi dvije akcije: jedna prikazuje formu, a druga obradu podataka.

```csharp
using Microsoft.AspNetCore.Mvc;
using Vjezba.App.Repositories.EF;
using Vjezba.Model;

namespace Vjezba.App.Controllers;

public class NoviEntitetController : Controller
{
    private readonly VjezbaDbContext _context;

    public NoviEntitetController(VjezbaDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var item = _context.RadneOprema.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(RadnaOprema model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var item = _context.RadneOprema.FirstOrDefault(x => x.Id == model.Id);
        if (item is null)
        {
            return NotFound();
        }

        item.Naziv = model.Naziv;
        item.InventarniBroj = model.InventarniBroj;
        item.SerijskiBroj = model.SerijskiBroj;
        item.Status = model.Status;
        item.LokacijaId = model.LokacijaId;
        item.ProizvodacId = model.ProizvodacId;
        item.KategorijaOpremeId = model.KategorijaOpremeId;

        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
```

Primjer iz projekta mozes prilagoditi prema obrascu koji koriste postojece akcije detalja i liste.

## 2. Create/Edit view s formom

View datoteka ide u `Views/NoviEntitet/Edit.cshtml` ili `Create.cshtml` i koristi `asp-for` tag helper.

```cshtml
@model Vjezba.Model.RadnaOprema

@{
    ViewData["Title"] = "Uredi opremu";
}

<form asp-action="Edit" method="post">
    <input type="hidden" asp-for="Id" />

    <div>
        <label asp-for="Naziv"></label>
        <input asp-for="Naziv" class="form-control" />
        <span asp-validation-for="Naziv"></span>
    </div>

    <div>
        <label asp-for="InventarniBroj"></label>
        <input asp-for="InventarniBroj" class="form-control" />
        <span asp-validation-for="InventarniBroj"></span>
    </div>

    <button type="submit">Spremi</button>
</form>
```

## 3. Model binding i slanje forme

ASP.NET Core MVC automatski mapira vrijednosti iz forme na model parametar u POST akciji.

```csharp
public IActionResult Create(Radnik model)
{
    if (!ModelState.IsValid)
    {
        return View(model);
    }

    _context.Radnici.Add(model);
    _context.SaveChanges();
    return RedirectToAction(nameof(Index));
}
```

Za validaciju koristi atribute iz modela, npr. `[Required]`, `[MaxLength]` i ostale EF/validation atribute.

## 4. Spremanje promjena s EF contextom

Kod izmjene prvo ucitaj postojeci zapis, zatim kopiraj vrijednosti iz modela i pozovi `SaveChanges()`.

```csharp
var item = _context.Lokacije.FirstOrDefault(x => x.Id == model.Id);
if (item is null)
{
    return NotFound();
}

item.Naziv = model.Naziv;
item.Adresa = model.Adresa;
_context.SaveChanges();
```

## 5. Primjeri iz projekta

Za ovaj projekt mozes koristiti isti obrazac za entitete kao sto su `RadnaOprema`, `Radnik`, `Lokacija` ili `Proizvodac`.

Primjer forme za radnika:

```cshtml
@model Vjezba.Model.Radnik

<form asp-action="Edit" method="post">
    <input type="hidden" asp-for="Id" />
    <input asp-for="Ime" />
    <input asp-for="Prezime" />
    <input asp-for="Email" />
    <button type="submit">Spremi</button>
</form>
```
