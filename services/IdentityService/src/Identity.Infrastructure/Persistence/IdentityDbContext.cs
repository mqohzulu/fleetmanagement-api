using Microsoft.EntityFrameworkCore;
using Identity.Domain.Entities;

namespace Identity.Infrastructure.Persistence
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(b =>
            {
                b.ToTable("Users");
                b.HasKey(u => u.Id);
                b.Property(u => u.Email).IsRequired();
                b.Property(u => u.Name).IsRequired();
                b.Property(u => u.PasswordHash).IsRequired();
            });
        }
    }
}
