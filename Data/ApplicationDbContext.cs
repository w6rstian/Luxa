using Microsoft.EntityFrameworkCore;
using Luxa.Models;

namespace Luxa.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            
        }
        public DbSet<UserModel> Users { get; set; }
    }
}
