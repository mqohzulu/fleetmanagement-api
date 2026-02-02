using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Operations.Infrastructure.Persistence
{
    public class DesignTimeOperationsDbContextFactory : IDesignTimeDbContextFactory<OperationsDbContext>
    {
        public OperationsDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<OperationsDbContext>();
            var connectionString = "Data Source=operations.db";
            builder.UseSqlite(connectionString);
            return new OperationsDbContext(builder.Options);
        }
    }
}
