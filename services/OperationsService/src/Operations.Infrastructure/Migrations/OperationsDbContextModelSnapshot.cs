using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Operations.Infrastructure.Persistence;

namespace Operations.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(OperationsDbContext))]
    partial class OperationsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("Operations.Domain.Entities.Trip", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<Guid>("VehicleId");
                b.Property<Guid>("DriverId");
                b.Property<DateTime>("StartedAt");
                b.Property<DateTime?>("EndedAt");
                b.Property<int>("Status");
                b.HasKey("Id");
                b.ToTable("Trips");
            });
        }
    }
}
