using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vjezba.Model
{
    public class RadnaOprema
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Naziv { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string InventarniBroj { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string SerijskiBroj { get; set; } = string.Empty;

        public DateTime DatumNabave { get; set; }

        public StatusOpreme Status { get; set; }

        public int LokacijaId { get; set; }
        public virtual Lokacija? Lokacija { get; set; }

        public int ProizvodacId { get; set; }
        public virtual Proizvodac? Proizvodac { get; set; }

        public int KategorijaOpremeId { get; set; }
        public virtual KategorijaOpreme? Kategorija { get; set; }

        public virtual ICollection<Odrzavanje> Odrzavanja { get; set; } = new List<Odrzavanje>();
        public virtual ICollection<ZaduzenjeOpreme> Zaduzenja { get; set; } = new List<ZaduzenjeOpreme>();
    }
}