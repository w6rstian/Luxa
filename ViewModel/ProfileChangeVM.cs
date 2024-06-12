using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Luxa.ViewModel
{
    public class ProfileChangeVM
    {
        [Display(Name = "Zdjęcie profilowe")]
        public IFormFile? Avatar { get; set; }

        [Display(Name = "Tło profilu")]
        public IFormFile? Background { get; set; }

        [Display(Name = "Opis profilu")]
        public string? Description { get; set; }
    }
}