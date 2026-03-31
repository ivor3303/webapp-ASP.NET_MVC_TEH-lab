using System;
using System.Collections.Generic;

namespace Vjezba.Model
{
	public class RadnaOprema
	{
		public int Id { get; set; }
		public string Naziv { get; set; } = string.Empty;
		public string InventarniBroj { get; set; } = string.Empty;
		public string SerijskiBroj { get; set; } = string.Empty;
		public DateTime DatumNabave { get; set; }
		public StatusOpreme Status { get; set; }
		public Lokacija Lokacija { get; set; } = new Lokacija();
		public Proizvodac Proizvodac { get; set; } = new Proizvodac();
		public KategorijaOpreme Kategorija { get; set; } = new KategorijaOpreme();
		public List<Odrzavanje> Odrzavanja { get; set; } = new List<Odrzavanje>();
	}
}

