using System.ComponentModel.DataAnnotations;

namespace Luxa.ViewModel
{
    public class SignUpVM
    {
        [Required(ErrorMessage = "Wpisz nazwę użytkownika")]
        [Display(Name = "Nazwa użytkownika")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Wpisz adres Email")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Adres Email")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Wpisz hasło użytkownika")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string? Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź Hasło")]
        [Compare("Password", ErrorMessage = "Hasła nie są zgodne")]
        public string? ConfirmPassword { get; set; }

    }
}
