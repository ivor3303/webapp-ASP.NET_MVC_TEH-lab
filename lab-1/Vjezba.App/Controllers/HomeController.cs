using Microsoft.AspNetCore.Mvc;
using Vjezba.App.Repositories;
using Vjezba.App.ViewModels;
using Vjezba.Model;

namespace Vjezba.App.Controllers;

public class HomeController : Controller
{
    private readonly RadnaOpremaRepository _radnaOpremaRepository;
    private readonly RadnikRepository _radnikRepository;
    private readonly OdrzavanjeRepository _odrzavanjeRepository;
    private readonly ServisniZahtjevRepository _servisniZahtjevRepository;
    private readonly LokacijaRepository _lokacijaRepository;

    public HomeController(
        RadnaOpremaRepository radnaOpremaRepository,
        RadnikRepository radnikRepository,
        OdrzavanjeRepository odrzavanjeRepository,
        ServisniZahtjevRepository servisniZahtjevRepository,
        LokacijaRepository lokacijaRepository)
    {
        _radnaOpremaRepository = radnaOpremaRepository;
        _radnikRepository = radnikRepository;
        _odrzavanjeRepository = odrzavanjeRepository;
        _servisniZahtjevRepository = servisniZahtjevRepository;
        _lokacijaRepository = lokacijaRepository;
    }

    public IActionResult Index()
    {
        var oprema = _radnaOpremaRepository.GetAll();
        var odrzavanja = _odrzavanjeRepository.GetAll();
        var servisniZahtjevi = _servisniZahtjevRepository.GetAll();
        var lokacije = _lokacijaRepository.GetAll();

        var rows = oprema
            .Select(oprema =>
            {
                var zadnjeOdrzavanje = oprema.Odrzavanja
                    .OrderByDescending(o => o.Datum)
                    .FirstOrDefault();

                return new HomeEquipmentRowViewModel
                {
                    Naziv = oprema.Naziv,
                    InventarniBroj = oprema.InventarniBroj,
                    Status = oprema.Status.ToString(),
                    LokacijaNaziv = oprema.Lokacija.Naziv,
                    ProizvodacNaziv = oprema.Proizvodac.Naziv,
                    BrojOdrzavanja = oprema.Odrzavanja.Count,
                    ZadnjeOdrzavanje = zadnjeOdrzavanje?.Datum
                };
            })
            .ToList();

        var model = new HomeDashboardViewModel
        {
            Oprema = rows,
            UkupnoOpreme = rows.Count,
            IspravnaOprema = oprema.Count(x => x.Status == StatusOpreme.Ispravna),
            NeispravnaOprema = oprema.Count(x => x.Status == StatusOpreme.Neispravna),
            OtpisanaOprema = oprema.Count(x => x.Status == StatusOpreme.Otpisana),
            OpremaUServisu = oprema.Count(x => x.Status == StatusOpreme.UServisu),
            BrojRadnika = _radnikRepository.GetAll().Count,
            UkupnoOdrzavanja = odrzavanja.Count,
            BrojServisnihZahtjeva = servisniZahtjevi.Count,
            BrojLokacija = rows.Select(x => x.LokacijaNaziv).Distinct(StringComparer.OrdinalIgnoreCase).Count(),
            BrojProizvodaca = rows.Select(x => x.ProizvodacNaziv).Distinct(StringComparer.OrdinalIgnoreCase).Count(),
            NajkriticnijaOprema = oprema
                .Select(x => new HomeCriticalEquipmentViewModel
                {
                    Id = x.Id,
                    Naziv = x.Naziv,
                    InventarniBroj = x.InventarniBroj,
                    Status = x.Status.ToString(),
                    Lokacija = x.Lokacija.Naziv,
                    BrojOdrzavanja = x.Odrzavanja.Count,
                    OtvorenihZahtjeva = servisniZahtjevi.Count(sz => sz.Oprema.Id == x.Id)
                })
                .OrderByDescending(x => x.Status == nameof(StatusOpreme.Neispravna))
                .ThenByDescending(x => x.OtvorenihZahtjeva)
                .ThenByDescending(x => x.BrojOdrzavanja)
                .Take(4)
                .ToList(),
            NedavnaOdrzavanja = odrzavanja
                .OrderByDescending(x => x.Datum)
                .Take(5)
                .Select(x => new HomeRecentMaintenanceViewModel
                {
                    Id = x.Id,
                    Datum = x.Datum,
                    Opis = x.Opis,
                    OpremaNaziv = x.Oprema.Naziv,
                    Izvrsio = $"{x.Izvrsio.Ime} {x.Izvrsio.Prezime}",
                    Cijena = x.Cijena
                })
                .ToList(),
            StanjePoLokacijama = lokacije
                .Select(l => new HomeLocationStatusViewModel
                {
                    Id = l.Id,
                    NazivLokacije = l.Naziv,
                    Ukupno = l.Oprema.Count,
                    Ispravna = l.Oprema.Count(x => x.Status == StatusOpreme.Ispravna),
                    Neispravna = l.Oprema.Count(x => x.Status == StatusOpreme.Neispravna),
                    Otpisana = l.Oprema.Count(x => x.Status == StatusOpreme.Otpisana),
                    UServisu = l.Oprema.Count(x => x.Status == StatusOpreme.UServisu)
                })
                .OrderByDescending(x => x.Neispravna)
                .ThenByDescending(x => x.UServisu)
                .ToList()
        };

        return View(model);
    }

    public IActionResult Error()
    {
        return View();
    }
}
