using System.ComponentModel.DataAnnotations;

namespace Vjezba.Model
{
	public enum StatusOpreme
	{
		[Display(Name = "Ispravna")]
		Ispravna,
		[Display(Name = "Neispravna")]
		Neispravna,
		[Display(Name = "U servisu")]
		UServisu,
		[Display(Name = "Otpisana")]
		Otpisana
	}
}


 