using Microsoft.EntityFrameworkCore;
using Maintenance.Domain.Entities;
using Maintenance.Infrastructure.Persistence;
using Maintenance.Infrastructure.Repositories;
using Xunit;

namespace Maintenance.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public async Task WorkOrderRepository_AddAndGetWorks()
        {
            var options = new DbContextOptionsBuilder<MaintenanceDbContext>()
                .UseInMemoryDatabase(databaseName: "MaintenanceTestDb")
                .Options;

            await using var context = new MaintenanceDbContext(options);
            var repo = new WorkOrderRepository(context);

            var wo = new WorkOrder(Guid.NewGuid(), "Replace brake pads", DateTime.UtcNow.AddDays(7));
            await repo.AddAsync(wo);

            var fetched = await repo.GetByIdAsync(wo.Id);
            Assert.NotNull(fetched);
            Assert.Equal(wo.Id, fetched!.Id);
        }
    }
}
