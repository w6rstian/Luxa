using System.ComponentModel.DataAnnotations;

namespace Luxa.ViewModel
{
    public class PasswordChangeVM
    {
        [Display(Name = "Stare hasło")]
        [Required(ErrorMessage = "Wpisz stare hasło")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; } = default!;

        [Display(Name = "Nowe hasło")]
        [Required(ErrorMessage = "Wpisz nowe hasło")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = default!;

        [Display(Name = "Potwierdź nowe hasło")]
        [Required(ErrorMessage = "Wpisz ponownie nowe hasło")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Hasła nie są zgodne")]
        public string ConfirmNewPassword { get; set; } = default!;

    }
}
