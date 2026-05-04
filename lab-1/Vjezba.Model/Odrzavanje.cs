using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [ForeignKey(nameof(IzvrsioId))]
        public virtual Radnik? Izvrsio { get; set; }

        public long TrajanjeTicks { get; set; }

        [NotMapped]
        public TimeSpan Trajanje
        {
            get => TimeSpan.FromTicks(TrajanjeTicks);
            set => TrajanjeTicks = value.Ticks;
        }

        public int OpremaId { get; set; }

        [ForeignKey(nameof(OpremaId))]
        public virtual RadnaOprema? Oprema { get; set; }

        [Required]
        [MaxLength(500)]
        public string Napomena { get; set; } = string.Empty;
    }
}