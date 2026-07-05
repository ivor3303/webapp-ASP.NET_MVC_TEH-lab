using System.ComponentModel.DataAnnotations;

namespace Vjezba.App.ViewModels;

public class AiAsistentViewModel
{
    public string UserText { get; set; } = string.Empty;

    [Required(ErrorMessage = "Naziv je obavezan")]
    public string Naziv { get; set; } = string.Empty;

    [Required(ErrorMessage = "Inventarni broj je obavezan")]
    public string InventarniBroj { get; set; } = string.Empty;

    [Required(ErrorMessage = "Serijski broj je obavezan")]
    public string SerijskiBroj { get; set; } = string.Empty;

    public int LokacijaId { get; set; }

    public int ProizvodacId { get; set; }

    public int KategorijaOpremeId { get; set; }
}
