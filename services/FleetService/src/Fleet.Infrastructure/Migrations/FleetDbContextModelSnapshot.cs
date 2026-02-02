using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Fleet.Infrastructure.Persistence;

namespace Fleet.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(FleetDbContext))]
    partial class FleetDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("Fleet.Domain.Entities.Vehicle", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string>("RegistrationNumber").IsRequired();
                b.Property<string>("Model").IsRequired();
                b.Property<int>("Status");
                b.Property<DateTime>("CreatedAt");
                b.HasKey("Id");
                b.ToTable("Vehicles");
            });
        }
    }
}
