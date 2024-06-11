using System.ComponentModel.DataAnnotations;

namespace Luxa.ViewModel
{
    public class ProfileChangeVM
    {
        [Required(ErrorMessage = "Zmodyfikuj opis profilu.")]
        [Display(Name = "Opis profilu")]
        public string? Description { get; set; }
    }
}
