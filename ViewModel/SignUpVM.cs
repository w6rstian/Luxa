using System.ComponentModel.DataAnnotations;

namespace Luxa.ViewModel
{
    public class SignUpVM
    {
        [Required(ErrorMessage ="Wpisz nazwę użytkownika")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Wpisz hasło użytkownika")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Hasła nie są zgodne")]
        public string? ConfirmPassword { get; set; }
        public string? Address { get; set; }

    }
}
