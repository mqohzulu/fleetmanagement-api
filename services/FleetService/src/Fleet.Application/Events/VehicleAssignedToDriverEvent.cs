namespace Fleet.Application.Events
{
    public record VehicleAssignedToDriverEvent(Guid VehicleId, Guid DriverId, DateTime AssignedAt);
}
