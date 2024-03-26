using Microsoft.EntityFrameworkCore;
using Luxa.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Luxa.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserModel>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            
        }
    }
}
