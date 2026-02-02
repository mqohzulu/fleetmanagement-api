using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Operations.Domain.Entities;

namespace Operations.Infrastructure.Configurations
{
    public class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.ToTable("Trips");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.VehicleId).IsRequired();
            builder.Property(t => t.DriverId).IsRequired();
            builder.Property(t => t.StartedAt).IsRequired();
            builder.Property(t => t.Status).IsRequired();
        }
    }
}
