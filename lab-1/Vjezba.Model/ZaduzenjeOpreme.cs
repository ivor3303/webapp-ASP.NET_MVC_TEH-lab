using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vjezba.Model
{
	public class ZaduzenjeOpreme
	{
		[Key]
		public int Id { get; set; }

		public int RadnikId { get; set; }

		[ForeignKey(nameof(RadnikId))]
		public Radnik Radnik { get; set; } = new Radnik();

		public int RadnaOpremaId { get; set; }

		[ForeignKey(nameof(RadnaOpremaId))]
		public RadnaOprema RadnaOprema { get; set; } = new RadnaOprema();

		public DateTime DatumZaduzenja { get; set; }

		public DateTime? DatumRazduzenja { get; set; }
	}
}