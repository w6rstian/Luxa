using Luxa.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luxa.Models
{
	public class UserModel
	{
		//klucz główny
		[Key]
		//automatycznie będzie dopisywać id przez bazę danych (chyba)
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		//dzięki tym opisom można wpisując w asp-for nazwę zmiennej można bez pisania w formsach wypisać to co tutaj
		[Display(Name = "Kategoria użytkownika")]
		public CategoryOfUser Category { get; set; }
		//atrybut do walidacji adresu e-mail (ale jeszcze nie wiem jak i czy to działa)
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Adres E-mail")]
		public string? Email { get; set; }
		[Display(Name = "Kraj")]
		public string? Country { get; set; }
		[Display(Name = "Imie")]
		[Required(ErrorMessage = """Pole "Imie" jest wymagane """)]
		public string FirstName { get; set; }
		[Display(Name = "Nazwisko")]
		[Required(ErrorMessage = """Pole "Nazwisko" jest wymagane """)]
		public string LastName { get; set; }
		//atrybut do numeru telefonu (też nie wiem jak i czy to działa)
		[DataType(DataType.PhoneNumber)]
		[Display(Name = "Numer telefonu")]
		public int? PhoneNumber { get; set; }
		[Display(Name = "Nazwa użytkownika")]
		[Required(ErrorMessage = """Pole "Nazwa użytkownika" jest wymagane """)]
		public string Nickname { get; set; }
		[DataType(DataType.Password)]
		[Required(ErrorMessage = """Pole "Hasło" jest wymagane """)]
		[Display(Name = "Hasło")]
		public string Password { get; set; }
	}
}
