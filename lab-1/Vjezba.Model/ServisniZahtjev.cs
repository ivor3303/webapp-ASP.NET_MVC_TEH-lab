using System;

namespace Vjezba.Model
{
	public class ServisniZahtjev
	{
		public int Id { get; set; }
		public DateTime DatumPrijave { get; set; }
		public string OpisKvara { get; set; } = string.Empty;
		public bool Hitno { get; set; }

		// New properties
		public string Status { get; set; } = "Otvoren";
		public RadnaOprema Oprema { get; set; } = new RadnaOprema();
		public string Komentar { get; set; } = string.Empty;
	}
}

