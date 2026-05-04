using System.ComponentModel.DataAnnotations;

namespace Vjezba.Model
{
	public class KategorijaOpreme
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(100)]
		public string Naziv { get; set; } = string.Empty;

		[Required]
		[MaxLength(500)]
		public string Opis { get; set; } = string.Empty;
		
	}
}

 