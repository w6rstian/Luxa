using System.ComponentModel.DataAnnotations;

namespace Luxa.ViewModel
{
	public class DataChangeVM
	{
		[Display(Name = "Email:")]
		public string? Email { get; set; } = default!;
		[Display(Name = "Kraj:")]
		public string? Country { get; set; } = default!;
		[Display(Name = "Imie:")]
		public string? FirstName { get; set; } = default!;
		[Display(Name = "Nazwisko:")]
		public string? LastName { get; set; } = default!;
	}
}
