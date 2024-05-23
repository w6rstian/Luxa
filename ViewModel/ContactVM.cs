using Luxa.Models;
using System.ComponentModel.DataAnnotations;

namespace Luxa.ViewModel
{
	public class ContactVM 
	{
		[Display(Name = "Wybierz cel kontaktu z listy poniżej")]
		public string Category { get; set; } = default!;
		[Display(Name = "Wybierz opcję")]
		public string DetailedCategory { get; set; } = default!;
		[Display(Name ="Wiadomość:")]
		[Required(ErrorMessage = "Pole Wiadomość jest wymagane.")]
		[MinLength(30, ErrorMessage = "Wiadomość musi zawierać co najmniej 30 znaków.")]
		public string Description { get; set; } = default!;
	}
}
