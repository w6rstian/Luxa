using System.ComponentModel.DataAnnotations;

namespace Luxa.ViewModel
{
	public class DataChangeVM
	{
		[Display(Name = "Email:")]
		[Required(ErrorMessage = "Wpisz adres Email")]
		[DataType(DataType.EmailAddress)]
		public string? Email { get; set; } = default!;
		[Required(ErrorMessage = "Wpisz kraj w którym mieszkasz")]
		[Display(Name = "Kraj:")]
		public string? Country { get; set; } = default!;
		[Required(ErrorMessage = "Wpisz swoje imie")]
		[Display(Name = "Imie:")]
		public string? FirstName { get; set; } = default!;
		[Required(ErrorMessage = "Wpisz swoje nazwisko")]
		[Display(Name = "Nazwisko:")]
		public string? LastName { get; set; } = default!;
		[Required(ErrorMessage = "Wpisz swój numer telefonu")]
		[Display(Name = "Numer telefonu:")]
		[DataType(DataType.PhoneNumber)]
		public string? PhoneNumber { get; set; } = default!;
	}
}
