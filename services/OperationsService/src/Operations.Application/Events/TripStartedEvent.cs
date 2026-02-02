namespace Operations.Application.Events
{
    public record TripStartedEvent(Guid TripId, Guid VehicleId, Guid DriverId, DateTime StartedAt);
}
