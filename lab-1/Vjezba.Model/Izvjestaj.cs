using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vjezba.Model
{
    public class Izvjestaj
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Naziv { get; set; } = string.Empty;

        public DateTime DatumIzrade { get; set; }

        [MaxLength(500)]
        public string Opis { get; set; } = string.Empty;

        public int RadnikId { get; set; }

        [ForeignKey(nameof(RadnikId))]
        public virtual Radnik? Radnik { get; set; }
    }
}