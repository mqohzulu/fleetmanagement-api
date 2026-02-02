using Microsoft.EntityFrameworkCore;
using Fleet.Domain.Entities;

namespace Fleet.Infrastructure.Persistence
{
    public class FleetDbContext : DbContext
    {
        public FleetDbContext(DbContextOptions<FleetDbContext> options) : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Vehicle>(b =>
            {
                b.ToTable("Vehicles");
                b.HasKey(v => v.Id);
                b.Property(v => v.RegistrationNumber).IsRequired();
                b.Property(v => v.Model).IsRequired();
                b.Property(v => v.Status).IsRequired();
            });
        }
    }
}
