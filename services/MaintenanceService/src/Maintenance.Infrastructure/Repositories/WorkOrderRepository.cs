using Microsoft.EntityFrameworkCore;
using Maintenance.Application.Interfaces;
using Maintenance.Domain.Entities;
using Maintenance.Infrastructure.Persistence;

namespace Maintenance.Infrastructure.Repositories
{
    public class WorkOrderRepository : IWorkOrderRepository
    {
        private readonly MaintenanceDbContext _db;

        public WorkOrderRepository(MaintenanceDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(WorkOrder workOrder, CancellationToken cancellationToken = default)
        {
            _db.WorkOrders.Add(workOrder);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<WorkOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _db.WorkOrders.FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(WorkOrder workOrder, CancellationToken cancellationToken = default)
        {
            _db.WorkOrders.Update(workOrder);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
