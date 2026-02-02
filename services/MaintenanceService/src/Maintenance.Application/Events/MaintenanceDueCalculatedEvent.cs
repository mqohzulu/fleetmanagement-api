namespace Maintenance.Application.Events
{
    public record MaintenanceDueCalculatedEvent(Guid WorkOrderId, Guid VehicleId, DateTime DueAt, DateTime CalculatedAt);
}
