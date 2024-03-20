using Microsoft.EntityFrameworkCore;
using Luxa.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Luxa.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            
        }
        public DbSet<UserModel> Users { get; set; }
    }
}
