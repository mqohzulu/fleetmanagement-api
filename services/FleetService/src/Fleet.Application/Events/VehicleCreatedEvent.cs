namespace Fleet.Application.Events
{
    public record VehicleCreatedEvent(Guid VehicleId, string RegistrationNumber, string Model, DateTime CreatedAt);
}
