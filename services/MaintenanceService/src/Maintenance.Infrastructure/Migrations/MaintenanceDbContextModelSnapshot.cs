using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Maintenance.Infrastructure.Persistence;

namespace Maintenance.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(MaintenanceDbContext))]
    partial class MaintenanceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("Maintenance.Domain.Entities.WorkOrder", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<Guid>("VehicleId");
                b.Property<string>("Description").IsRequired();
                b.Property<int>("Status");
                b.Property<DateTime>("CreatedAt");
                b.Property<DateTime?>("DueAt");
                b.Property<DateTime?>("CompletedAt");
                b.HasKey("Id");
                b.ToTable("WorkOrders");
            });
        }
    }
}
