namespace Operations.Application.Events
{
    public record TripEndedEvent(Guid TripId, Guid VehicleId, Guid DriverId, DateTime StartedAt, DateTime EndedAt);
}
