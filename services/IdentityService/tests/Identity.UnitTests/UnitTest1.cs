using System.Text;
using Microsoft.EntityFrameworkCore;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Repositories;
using Xunit;

namespace Identity.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public async Task UserRepository_AddAndGetWorks()
        {
            var options = new DbContextOptionsBuilder<IdentityDbContext>()
                .UseInMemoryDatabase(databaseName: "IdentityTestDb")
                .Options;

            await using var context = new IdentityDbContext(options);
            var repo = new UserRepository(context);

            var pwdHash = Convert.ToBase64String(Encoding.UTF8.GetBytes("pass"));
            var user = new User("alice@example.com", "Alice", pwdHash);
            await repo.AddAsync(user);

            var fetched = await repo.GetByEmailAsync("alice@example.com");
            Assert.NotNull(fetched);
            Assert.Equal(user.Email, fetched!.Email);
        }
    }
}
