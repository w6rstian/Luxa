using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
    public class User : IdentityUser
    {
        //klucz główny
        [Key]
        public int Id { get; set; }
        //atrybut do walidacji adresu e-mail
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Country  { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //atrybut do numeru telefonu
        [DataType(DataType.PhoneNumber)]
        public int PhoneNumber { get; set; }
    }
}
