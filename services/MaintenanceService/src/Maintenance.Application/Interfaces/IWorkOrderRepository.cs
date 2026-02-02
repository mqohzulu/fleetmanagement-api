using Maintenance.Domain.Entities;

namespace Maintenance.Application.Interfaces
{
    public interface IWorkOrderRepository
    {
        Task AddAsync(WorkOrder workOrder, CancellationToken cancellationToken = default);
        Task<WorkOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateAsync(WorkOrder workOrder, CancellationToken cancellationToken = default);
    }
}
