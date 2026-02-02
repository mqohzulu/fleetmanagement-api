using Fleet.Domain.Entities;

namespace Fleet.Application.Interfaces
{
    public interface IVehicleRepository
    {
        Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
        Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
    }
}
