namespace Vjezba.App.DTOs;

public class ServisniZahtjevDTO
{
    public int Id { get; set; }
    public DateTime DatumPrijave { get; set; }
    public string OpisKvara { get; set; } = string.Empty;
    public bool Hitno { get; set; }
    public string Komentar { get; set; } = string.Empty;
    public RadnaOpremaDTO? Oprema { get; set; }
}