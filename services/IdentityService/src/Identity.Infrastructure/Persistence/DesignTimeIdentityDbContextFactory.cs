using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Identity.Infrastructure.Persistence
{
    // Provides a design-time factory for EF Core tools (migrations)
    public class DesignTimeIdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<IdentityDbContext>();
            // Default to a local SQLite file for migrations if no connection string provided
            var connectionString = "Data Source=identity.db";
            builder.UseSqlite(connectionString);
            return new IdentityDbContext(builder.Options);
        }
    }
}
