using Vjezba.Model;

namespace Vjezba.App.ViewModels;

public class GlobalSearchViewModel
{
    public string Query { get; set; } = string.Empty;
    public List<RadnaOprema> RadnaOprema { get; set; } = new();
    public List<Radnik> Radnici { get; set; } = new();
    public List<Lokacija> Lokacije { get; set; } = new();
    public List<Proizvodac> Proizvodaci { get; set; } = new();
    public List<KategorijaOpreme> KategorijeOpreme { get; set; } = new();
    public List<Odrzavanje> Odrzavanja { get; set; } = new();
    public List<ServisniZahtjev> ServisniZahtjevi { get; set; } = new();
    public int TotalResults { get; set; }
}
