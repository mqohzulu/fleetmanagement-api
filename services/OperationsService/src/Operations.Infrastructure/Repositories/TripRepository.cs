using Microsoft.EntityFrameworkCore;
using Operations.Application.Interfaces;
using Operations.Domain.Entities;
using Operations.Infrastructure.Persistence;

namespace Operations.Infrastructure.Repositories
{
    public class TripRepository : ITripRepository
    {
        private readonly OperationsDbContext _db;

        public TripRepository(OperationsDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Trip trip, CancellationToken cancellationToken = default)
        {
            _db.Trips.Add(trip);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<Trip?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _db.Trips.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(Trip trip, CancellationToken cancellationToken = default)
        {
            _db.Trips.Update(trip);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
