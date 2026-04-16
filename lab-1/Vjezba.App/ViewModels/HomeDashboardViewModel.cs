namespace Vjezba.App.ViewModels;

public class HomeDashboardViewModel
{
    public IReadOnlyList<HomeEquipmentRowViewModel> Oprema { get; set; } = Array.Empty<HomeEquipmentRowViewModel>();
    public int UkupnoOpreme { get; set; }
    public int IspravnaOprema { get; set; }
    public int NeispravnaOprema { get; set; }
    public int OtpisanaOprema { get; set; }
    public int OpremaUServisu { get; set; }
    public int BrojRadnika { get; set; }
    public int UkupnoOdrzavanja { get; set; }
    public int BrojServisnihZahtjeva { get; set; }
    public int BrojLokacija { get; set; }
    public int BrojProizvodaca { get; set; }

    public IReadOnlyList<HomeCriticalEquipmentViewModel> NajkriticnijaOprema { get; set; } = Array.Empty<HomeCriticalEquipmentViewModel>();
    public IReadOnlyList<HomeRecentMaintenanceViewModel> NedavnaOdrzavanja { get; set; } = Array.Empty<HomeRecentMaintenanceViewModel>();
    public IReadOnlyList<HomeLocationStatusViewModel> StanjePoLokacijama { get; set; } = Array.Empty<HomeLocationStatusViewModel>();
}

public class HomeCriticalEquipmentViewModel
{
    public int Id { get; set; }
    public string Naziv { get; set; } = string.Empty;
    public string InventarniBroj { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Lokacija { get; set; } = string.Empty;
    public int BrojOdrzavanja { get; set; }
    public int OtvorenihZahtjeva { get; set; }
}

public class HomeRecentMaintenanceViewModel
{
    public int Id { get; set; }
    public DateTime Datum { get; set; }
    public string Opis { get; set; } = string.Empty;
    public string OpremaNaziv { get; set; } = string.Empty;
    public string Izvrsio { get; set; } = string.Empty;
    public decimal Cijena { get; set; }
}

public class HomeLocationStatusViewModel
{
    public int Id { get; set; }
    public string NazivLokacije { get; set; } = string.Empty;
    public int Ukupno { get; set; }
    public int Ispravna { get; set; }
    public int Neispravna { get; set; }
    public int Otpisana { get; set; }
    public int UServisu { get; set; }
}
