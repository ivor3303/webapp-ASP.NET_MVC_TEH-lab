using System.ComponentModel.DataAnnotations;

namespace Vjezba.Model
{
	public class Proizvodac
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(100)]
		public string Naziv { get; set; } = string.Empty;

		[Required]
		[MaxLength(100)]
		public string Drzava { get; set; } = string.Empty;

		[Required]
		[MaxLength(200)]
		public string KontaktEmail { get; set; } = string.Empty;
		
	}
}


