using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vjezba.Model
{
	public class Lokacija
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(100)]
		public string Naziv { get; set; } = string.Empty;

		[Required]
		[MaxLength(300)]
		public string Adresa { get; set; } = string.Empty;

		public DateTime? DeletedAt { get; set; }

		public virtual ICollection<RadnaOprema> Oprema { get; set; } = new List<RadnaOprema>();
	
	}
}

