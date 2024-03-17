using System.ComponentModel.DataAnnotations;

namespace Luxa.Models
{
    public class UserModel
    {
        //klucz główny
        [Key]
        public int Id { get; set; }
        //atrybut do walidacji adresu e-mail
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        public string? Country { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //atrybut do numeru telefonu
        [DataType(DataType.PhoneNumber)]
        public int? PhoneNumber { get; set; }
        [Display(Name="Nazwa użytkownika")]
        public string Nickname { get; set;}
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set;}
    }
}
