using System.ComponentModel.DataAnnotations;

namespace Luxa.ViewModel
{
    public class CreateUserVM
    {
        [Display(Name = "Kraj")]
        public string? Country { get; set; }
        [Display(Name = "Imię")]
        public string? FirstName { get; set; }
        [Display(Name = "Nazwisko")]
        public string? LastName { get; set; }
        [Display(Name = "Numer telefonu")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Wprowadź Hasło w pierwszym polu")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; } = default!;
        [Required(ErrorMessage = "Wprowadź Hasło w drugim polu")]
        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź Hasło")]
        [Compare("Password", ErrorMessage = "Hasła nie są zgodne")]
        public string ConfirmPassword { get; set; } = default!;
        [Display(Name = "Rola użytkownika")]
        public string Role { get; set; } = default!;
        [Required(ErrorMessage = "Wprowadź adres Email")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Adres Email")]
        public string Email { get; set; } = default!;
        [Required(ErrorMessage = "Wpisz nazwę użytkownika")]
        [Display(Name = "Nazwa użytkownika")]
        public string Username { get; set; } = default!;

    }

}
