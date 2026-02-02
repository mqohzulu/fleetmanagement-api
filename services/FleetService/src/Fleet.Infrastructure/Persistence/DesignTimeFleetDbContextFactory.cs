using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Fleet.Infrastructure.Persistence
{
    public class DesignTimeFleetDbContextFactory : IDesignTimeDbContextFactory<FleetDbContext>
    {
        public FleetDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<FleetDbContext>();
            var connectionString = "Data Source=fleet.db";
            builder.UseSqlite(connectionString);
            return new FleetDbContext(builder.Options);
        }
    }
}
