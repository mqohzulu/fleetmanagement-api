using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Maintenance.Infrastructure.Persistence
{
    public class DesignTimeMaintenanceDbContextFactory : IDesignTimeDbContextFactory<MaintenanceDbContext>
    {
        public MaintenanceDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<MaintenanceDbContext>();
            var connectionString = "Data Source=maintenance.db";
            builder.UseSqlite(connectionString);
            return new MaintenanceDbContext(builder.Options);
        }
    }
}
