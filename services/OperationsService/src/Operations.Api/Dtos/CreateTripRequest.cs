using System;

namespace Operations.Api.Dtos
{
    public record CreateTripRequest(Guid VehicleId, Guid DriverId, DateTime? StartedAt = null);
}
