# Skill za izradu liste

Ova smjernica opisuje kako napraviti novu stranicu za popis podataka u aplikaciji **Evidencija odrzavanja radne opreme u zastiti na radu**.

## 1. Stvori controller s Index akcijom

Controller obicno prima EF repozitorij putem dependency injectiona i u `Index()` dohvatiti podatke.

```csharp
using Microsoft.AspNetCore.Mvc;
using Vjezba.App.Repositories.EF;

namespace Vjezba.App.Controllers;

public class NoviEntitetController : Controller
{
    private readonly EFNoviEntitetRepository _repository;

    public NoviEntitetController(EFNoviEntitetRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index()
    {
        var items = _repository.GetAll();
        return View(items);
    }
}
```

Primjer iz projekta je [RadnaOpremaController.cs](../../Vjezba.App/Controllers/RadnaOpremaController.cs) ili [RadnikController.cs](../../Vjezba.App/Controllers/RadnikController.cs).

## 2. Stvori Index view s tablicom

View datoteka treba biti u mapi `Views/NoviEntitet/Index.cshtml` i koristiti model kolekcije.

```cshtml
@model IReadOnlyList<Vjezba.Model.RadnaOprema>

@{
    ViewData["Title"] = "Popis opreme";
}

<h1>Popis opreme</h1>

<table class="data-table">
    <thead>
        <tr>
            <th>Naziv</th>
            <th>Inventarni broj</th>
            <th>Status</th>
            <th></th>
        </tr>
    </thead>
    <tbody>
    @foreach (var item in Model)
    {
        <tr>
            <td>@item.Naziv</td>
            <td>@item.InventarniBroj</td>
            <td>@item.Status</td>
            <td>
                <a asp-controller="RadnaOprema" asp-action="Details" asp-route-id="@item.Id">Detalji</a>
            </td>
        </tr>
    }
    </tbody>
</table>
```

Primjer stila tablice mozes vidjeti u [Views/RadnaOprema/Index.cshtml](../../Vjezba.App/Views/RadnaOprema/Index.cshtml).

## 3. Linkovi s liste na detalje

Za navigaciju s liste prema detaljima koristi `asp-controller`, `asp-action` i `asp-route-id`.

```cshtml
<a asp-controller="Lokacija" asp-action="Details" asp-route-id="@item.Id">Detalji</a>
```

To je isti obrazac koji se koristi u postojecim viewovima za opremu, lokacije i radnike.

## 4. Dodavanje u navigacijski izbornik

Novi popis treba dodati i u [Views/Shared/_Layout.cshtml](../../Vjezba.App/Views/Shared/_Layout.cshtml) kako bi bio dostupan iz glavnog izbornika.

```cshtml
<a asp-controller="NoviEntitet" asp-action="Index">Novi popis</a>
```

Ako zelis zadrzati konzistentnost, koristi isti obrazac aktivnih linkova kao u postojecem layoutu.

## 5. Primjer prema postojecim controllerima i viewovima

Primjer iz projekta za listu radnika:

```csharp
public IActionResult Index()
{
    var items = _repository.GetAll();
    return View(items);
}
```

```cshtml
@model IReadOnlyList<Vjezba.Model.Radnik>

<table class="data-table">
    <tbody>
    @foreach (var radnik in Model)
    {
        <tr>
            <td>@radnik.Ime @radnik.Prezime</td>
            <td>
                <a asp-controller="Radnik" asp-action="Details" asp-route-id="@radnik.Id">Profil</a>
            </td>
        </tr>
    }
    </tbody>
</table>
```
