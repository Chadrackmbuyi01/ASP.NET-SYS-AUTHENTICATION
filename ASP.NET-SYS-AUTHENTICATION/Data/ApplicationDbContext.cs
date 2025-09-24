using Microsoft.EntityFrameworkCore;
using ASP.NET_SYS_AUTHENTICATION.Models;

namespace ASP.NET_SYS_AUTHENTICATION.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
                entity.Property(u => u.Firstname).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Lastname).IsRequired().HasMaxLength(100);
            });
        }
    }
}