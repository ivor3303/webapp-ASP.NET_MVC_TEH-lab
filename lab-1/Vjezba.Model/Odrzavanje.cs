using System;

namespace Vjezba.Model
{
	public class Odrzavanje
	{
		public int Id { get; set; }
		public DateTime Datum { get; set; }
		public string Opis { get; set; } = string.Empty;
		public decimal Cijena { get; set; }
		public Radnik Izvrsio { get; set; } = new Radnik();

		public TimeSpan Trajanje { get; set; }
		public RadnaOprema Oprema { get; set; } = new RadnaOprema();
		public string Napomena { get; set; } = string.Empty;
	}
}

