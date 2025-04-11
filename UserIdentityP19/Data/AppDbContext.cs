using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserIdentityP19.Models;

namespace UserIdentityP19.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Student> Students { get; set; }
    }
}
