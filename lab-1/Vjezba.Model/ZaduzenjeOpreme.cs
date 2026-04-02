using System;

namespace Vjezba.Model
{
	public class ZaduzenjeOpreme
	{
		public int Id { get; set; }
		public Radnik Radnik { get; set; } = new Radnik();
		public RadnaOprema RadnaOprema { get; set; } = new RadnaOprema();
		public DateTime DatumZaduzenja { get; set; }
		public DateTime? DatumRazduzenja { get; set; }
	}
}