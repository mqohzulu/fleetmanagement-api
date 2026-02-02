using Microsoft.EntityFrameworkCore;
using Operations.Domain.Entities;
using Operations.Infrastructure.Configurations;

namespace Operations.Infrastructure.Persistence
{
    public class OperationsDbContext : DbContext
    {
        public OperationsDbContext(DbContextOptions<OperationsDbContext> options) : base(options)
        {
        }

        public DbSet<Trip> Trips { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new TripConfiguration());
        }
    }
}
