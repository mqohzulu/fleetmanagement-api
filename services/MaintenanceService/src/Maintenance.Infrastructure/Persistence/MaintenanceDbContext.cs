using Microsoft.EntityFrameworkCore;
using Maintenance.Domain.Entities;

namespace Maintenance.Infrastructure.Persistence
{
    public class MaintenanceDbContext : DbContext
    {
        public MaintenanceDbContext(DbContextOptions<MaintenanceDbContext> options) : base(options)
        {
        }

        public DbSet<WorkOrder> WorkOrders { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<WorkOrder>(b =>
            {
                b.ToTable("WorkOrders");
                b.HasKey(w => w.Id);
                b.Property(w => w.VehicleId).IsRequired();
                b.Property(w => w.Description).IsRequired();
                b.Property(w => w.Status).IsRequired();
            });
        }
    }
}
