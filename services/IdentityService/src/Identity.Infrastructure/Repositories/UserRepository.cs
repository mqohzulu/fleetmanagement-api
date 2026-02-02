using Microsoft.EntityFrameworkCore;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence;

namespace Identity.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityDbContext _db;

        public UserRepository(IdentityDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
