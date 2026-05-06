using System;
using System.ComponentModel.DataAnnotations;

namespace Vjezba.Model
{
    public class Odrzavanje
    {
        [Key]
        public int Id { get; set; }

        public DateTime Datum { get; set; }

        [Required]
        [MaxLength(500)]
        public string Opis { get; set; } = string.Empty;

        public decimal Cijena { get; set; }

        public int IzvrsioId { get; set; }
        public virtual Radnik? Izvrsio { get; set; }

        public long TrajanjeTicks { get; set; }

        public int OpremaId { get; set; }
        public virtual RadnaOprema? Oprema { get; set; }

        [Required]
        [MaxLength(500)]
        public string Napomena { get; set; } = string.Empty;
    }
}