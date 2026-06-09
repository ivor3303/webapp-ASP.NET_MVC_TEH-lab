namespace Vjezba.App.DTOs;

public class ZaduzenjeOpremeDTO
{
    public int Id { get; set; }
    public DateTime DatumZaduzenja { get; set; }
    public DateTime? DatumRazduzenja { get; set; }
    public RadnikDTO? Radnik { get; set; }
    public RadnaOpremaDTO? RadnaOprema { get; set; }
}