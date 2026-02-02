using Microsoft.EntityFrameworkCore;
using Fleet.Application.Interfaces;
using Fleet.Domain.Entities;
using Fleet.Infrastructure.Persistence;

namespace Fleet.Infrastructure.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly FleetDbContext _db;

        public VehicleRepository(FleetDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
        {
            _db.Vehicles.Add(vehicle);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
        {
            _db.Vehicles.Update(vehicle);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
