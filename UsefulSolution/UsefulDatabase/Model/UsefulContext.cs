using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UsefulDatabase.Model.Roles;
using UsefulDatabase.Model.Users;

namespace UsefulDatabase.Model
{
    public class UsefulContext : IdentityDbContext<User, Role, string>
    {
        public UsefulContext(DbContextOptions<UsefulContext> options)
            : base(options)
        { }

        public DbSet<User> ApplicationUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
