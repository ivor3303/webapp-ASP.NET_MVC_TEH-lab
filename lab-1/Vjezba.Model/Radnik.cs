using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vjezba.Model
{
	public class Radnik
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(100)]
		public string Ime { get; set; } = string.Empty;

		[Required]
		[MaxLength(100)]
		public string Prezime { get; set; } = string.Empty;

		[Required]
		[MaxLength(100)]
		public string RadnoMjesto { get; set; } = string.Empty;

		[Required]
		[MaxLength(200)]
		public string Email { get; set; } = string.Empty;

		[Required]
		[MaxLength(50)]
		public string Telefon { get; set; } = string.Empty;

		public DateTime DatumZaposlenja { get; set; }

		public bool Aktivan { get; set; } = true;

		public virtual ICollection<ServisniZahtjev> ServisniZahtjevi { get; set; } = new List<ServisniZahtjev>();
		public virtual ICollection<ZaduzenjeOpreme> Zaduzenja { get; set; } = new List<ZaduzenjeOpreme>();
	}
}

