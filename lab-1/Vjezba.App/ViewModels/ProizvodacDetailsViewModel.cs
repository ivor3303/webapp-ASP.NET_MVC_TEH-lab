using Vjezba.Model;

namespace Vjezba.App.ViewModels;

public class ProizvodacDetailsViewModel
{
    public Proizvodac Proizvodac { get; set; } = new();
    public IReadOnlyList<RadnaOprema> Oprema { get; set; } = Array.Empty<RadnaOprema>();
}
