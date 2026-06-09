using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vjezba.Model
{
    public class RadnaOprema
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv je obavezan")]
        [MaxLength(100, ErrorMessage = "Naziv ne smije biti dulji od 100 znakova")]
        public string Naziv { get; set; } = string.Empty;

        [Required(ErrorMessage = "Inventarni broj je obavezan")]
        [MaxLength(100, ErrorMessage = "Inventarni broj ne smije biti dulji od 100 znakova")]
        public string InventarniBroj { get; set; } = string.Empty;

        [Required(ErrorMessage = "Serijski broj je obavezan")]
        [MaxLength(100, ErrorMessage = "Serijski broj ne smije biti dulji od 100 znakova")]
        public string SerijskiBroj { get; set; } = string.Empty;

        [Required(ErrorMessage = "Datum nabave je obavezan")]
        public DateTime DatumNabave { get; set; }

        public StatusOpreme Status { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Molimo odaberite lokaciju")]
        public int LokacijaId { get; set; }
        public virtual Lokacija? Lokacija { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Molimo odaberite lokaciju")]
        public int ProizvodacId { get; set; }
        public virtual Proizvodac? Proizvodac { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Molimo odaberite lokaciju")]
        public int KategorijaOpremeId { get; set; }
        public virtual KategorijaOpreme? Kategorija { get; set; }

        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public virtual ICollection<Odrzavanje> Odrzavanja { get; set; } = new List<Odrzavanje>();
        public virtual ICollection<ZaduzenjeOpreme> Zaduzenja { get; set; } = new List<ZaduzenjeOpreme>();
    }
}