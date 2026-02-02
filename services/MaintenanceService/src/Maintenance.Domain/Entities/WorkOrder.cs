using Maintenance.Domain.Enums;

namespace Maintenance.Domain.Entities
{
    public class WorkOrder
    {
        public Guid Id { get; private set; }
        public Guid VehicleId { get; private set; }
        public string Description { get; private set; } = null!;
        public WorkOrderStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? DueAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        private WorkOrder() { }

        public WorkOrder(Guid vehicleId, string description, DateTime? dueAt = null)
        {
            Id = Guid.NewGuid();
            VehicleId = vehicleId;
            Description = description;
            CreatedAt = DateTime.UtcNow;
            DueAt = dueAt;
            Status = WorkOrderStatus.Open;
        }

        public void MarkInProgress() => Status = WorkOrderStatus.InProgress;
        public void Complete(DateTime completedAt)
        {
            Status = WorkOrderStatus.Completed;
            CompletedAt = completedAt;
        }
    }
}
