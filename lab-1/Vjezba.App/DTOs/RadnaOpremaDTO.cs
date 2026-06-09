namespace Vjezba.App.DTOs;

public class RadnaOpremaDTO
{
    public int Id { get; set; }
    public string Naziv { get; set; } = string.Empty;
    public string InventarniBroj { get; set; } = string.Empty;
    public string SerijskiBroj { get; set; } = string.Empty;
    public DateTime DatumNabave { get; set; }
    public string Status { get; set; } = string.Empty;
    public LokacijaDTO? Lokacija { get; set; }
    public ProizvodacDTO? Proizvodac { get; set; }
    public KategorijaOpremeDTO? Kategorija { get; set; }
}