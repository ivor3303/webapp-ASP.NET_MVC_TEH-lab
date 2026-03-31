using System.Collections.Generic;

namespace Vjezba.Model
{
	public class Lokacija
	{
		public int Id { get; set; }
		public string Naziv { get; set; } = string.Empty;
		public string Adresa { get; set; } = string.Empty;
		public List<RadnaOprema> Oprema { get; set; } = new List<RadnaOprema>();
	
	}
}

