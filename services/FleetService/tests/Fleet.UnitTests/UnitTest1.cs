using Microsoft.EntityFrameworkCore;
using Fleet.Domain.Entities;
using Fleet.Infrastructure.Persistence;
using Fleet.Infrastructure.Repositories;
using Xunit;

namespace Fleet.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public async Task VehicleRepository_AddAndGetWorks()
        {
            var options = new DbContextOptionsBuilder<FleetDbContext>()
                .UseInMemoryDatabase(databaseName: "FleetTestDb")
                .Options;

            await using var context = new FleetDbContext(options);
            var repo = new VehicleRepository(context);

            var v = new Vehicle("ABC-123", "Toyota Prius");
            await repo.AddAsync(v);

            var fetched = await repo.GetByIdAsync(v.Id);
            Assert.NotNull(fetched);
            Assert.Equal(v.Id, fetched!.Id);
        }
    }
}
