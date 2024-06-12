using System.ComponentModel.DataAnnotations;

namespace Luxa.ViewModel
{
    public class ProfileChangeVM
    {
        [Required(ErrorMessage = "Zmodyfikuj zdjęcie profilowe.")]
        [Display(Name = "Zdjęcie profilowe")]
        public IFormFile Avatar { get; set; }

        [Required(ErrorMessage = "Zmodyfikuj tło profilu.")]
        [Display(Name = "Tło profilu")]
        public IFormFile Background { get; set; }

        [Required(ErrorMessage = "Zmodyfikuj opis profilu.")]
        [Display(Name = "Opis profilu")]
        public string? Description { get; set; }
    }
}
