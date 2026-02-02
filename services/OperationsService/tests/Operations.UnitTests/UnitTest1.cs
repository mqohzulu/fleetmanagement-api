using Microsoft.EntityFrameworkCore;
using Operations.Domain.Entities;
using Operations.Infrastructure.Persistence;
using Operations.Infrastructure.Repositories;
using Xunit;

namespace Operations.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Repository_AddAndGetWorks()
        {
            var options = new DbContextOptionsBuilder<OperationsDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            await using var context = new OperationsDbContext(options);
            var repo = new TripRepository(context);

            var trip = new Trip(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);
            await repo.AddAsync(trip);

            var fetched = await repo.GetByIdAsync(trip.Id);
            Assert.NotNull(fetched);
            Assert.Equal(trip.Id, fetched!.Id);
        }
    }
}
