using Operations.Domain.Entities;

namespace Operations.Application.Interfaces
{
    public interface ITripRepository
    {
        Task AddAsync(Trip trip, CancellationToken cancellationToken = default);
        Task<Trip?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateAsync(Trip trip, CancellationToken cancellationToken = default);
    }
}
