using System;
using System.ComponentModel.DataAnnotations;

namespace Vjezba.Model
{
    public class ServisniZahtjev
    {
        [Key]
        public int Id { get; set; }

        public DateTime DatumPrijave { get; set; }

        [Required]
        [MaxLength(500)]
        public string OpisKvara { get; set; } = string.Empty;

        public bool Hitno { get; set; }

        public int OpremaId { get; set; }
        public virtual RadnaOprema? Oprema { get; set; }

        [Required]
        [MaxLength(500)]
        public string Komentar { get; set; } = string.Empty;
    }
}