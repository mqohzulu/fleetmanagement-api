using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Identity.Infrastructure.Persistence;

namespace Identity.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(IdentityDbContext))]
    partial class IdentityDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("Identity.Domain.Entities.User", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string>("Email").IsRequired();
                b.Property<string>("Name").IsRequired();
                b.Property<string>("PasswordHash").IsRequired();
                b.Property<DateTime>("CreatedAt");
                b.HasKey("Id");
                b.ToTable("Users");
            });
        }
    }
}
