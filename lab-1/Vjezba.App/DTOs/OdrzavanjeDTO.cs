namespace Vjezba.App.DTOs;

public class OdrzavanjeDTO
{
    public int Id { get; set; }
    public DateTime Datum { get; set; }
    public string Opis { get; set; } = string.Empty;
    public decimal Cijena { get; set; }
    public string Napomena { get; set; } = string.Empty;
    public RadnaOpremaDTO? Oprema { get; set; }
    public RadnikDTO? Izvrsio { get; set; }
}