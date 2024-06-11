using System.ComponentModel.DataAnnotations;

namespace Luxa.ViewModel
{
    public class PrivacyChangeVM
    {
        [Required(ErrorMessage = "Wybierz widoczność profilu.")]
        [Display(Name = "Prywatny profil")]
        public bool IsPrivate { get; set; }
    }
}
