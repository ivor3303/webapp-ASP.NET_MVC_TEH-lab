namespace Vjezba.App.ViewModels;

public class HomeEquipmentRowViewModel
{
    public string Naziv { get; set; } = string.Empty;
    public string InventarniBroj { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string LokacijaNaziv { get; set; } = string.Empty;
    public string ProizvodacNaziv { get; set; } = string.Empty;
    public int BrojOdrzavanja { get; set; }
    public DateTime? ZadnjeOdrzavanje { get; set; }
}
