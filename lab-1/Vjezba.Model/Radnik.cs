namespace Vjezba.Model
{
	public class Radnik
	{
		public int Id { get; set; }
		public string Ime { get; set; } = string.Empty;
		public string Prezime { get; set; } = string.Empty;
		public string RadnoMjesto { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Telefon { get; set; } = string.Empty;
		public DateTime DatumZaposlenja { get; set; }
		public bool Aktivan { get; set; } = true;
		public List<ServisniZahtjev> ServisniZahtjevi { get; set; } = new List<ServisniZahtjev>();
		public List<ZaduzenjeOpreme> Zaduzenja { get; set; } = new List<ZaduzenjeOpreme>();
	}
}

