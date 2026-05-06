using System;
using System.ComponentModel.DataAnnotations;

namespace Vjezba.Model
{
    public class ZaduzenjeOpreme
    {
        [Key]
        public int Id { get; set; }

        public int RadnikId { get; set; }
        public virtual Radnik? Radnik { get; set; }

        public int RadnaOpremaId { get; set; }
        public virtual RadnaOprema? RadnaOprema { get; set; }

        public DateTime DatumZaduzenja { get; set; }

        public DateTime? DatumRazduzenja { get; set; }
    }
}