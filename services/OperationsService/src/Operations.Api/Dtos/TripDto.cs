using System;
using Operations.Domain.Enums;

namespace Operations.Api.Dtos
{
    public record TripDto(Guid Id, Guid VehicleId, Guid DriverId, DateTime StartedAt, DateTime? EndedAt, TripStatus Status);
}
