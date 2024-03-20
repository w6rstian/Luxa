using Microsoft.AspNetCore.Identity;

namespace Luxa.Models
{
    public class AppUser : IdentityUser
    {
        public string? Address { get; set; }
    }
}
