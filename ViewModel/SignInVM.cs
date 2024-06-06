using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Luxa.ViewModel
{
    public class SignInVM
    {
        
        [Required(ErrorMessage = "Wpisz nazwę użytkownika")]
		[Display(Name = "Nazwa użytkownika")]
		public string? UserName { get; set; }
		[Display(Name = "Hasło")]
		[Required(ErrorMessage = "Wpisz hasło użytkownika")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Display(Name = "Zapamiętaj mnie")]
        public bool RememberMe { get; set; }
	}
}
