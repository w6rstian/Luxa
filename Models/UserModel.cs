using Luxa.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luxa.Models
{
	public class UserModel
	{
		//klucz główny
		[Key]
		//automatycznie będzie dopisywać id
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Display(Name = "Kategoria użytkownika")]
		public CategoryOfUser Category { get; set; }
		//atrybut do walidacji adresu e-mail
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Adres E-mail")]
		public string? Email { get; set; }
		[Display(Name = "Kraj")]
		public string? Country { get; set; }
		[Display(Name = "Imie")]
		public string FirstName { get; set; }
		[Display(Name = "Nazwisko")]
		public string LastName { get; set; }
		//atrybut do numeru telefonu
		[DataType(DataType.PhoneNumber)]
		[Display(Name = "Numer telefonu")]
		public int? PhoneNumber { get; set; }
		[Display(Name = "Nazwa użytkownika")]
		public string Nickname { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Hasło")]
		public string Password { get; set; }
	}
}
