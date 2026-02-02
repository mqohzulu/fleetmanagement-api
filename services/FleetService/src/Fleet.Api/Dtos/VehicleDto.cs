using System;
using Fleet.Domain.Enums;

namespace Fleet.Api.Dtos
{
    public record VehicleDto(Guid Id, string RegistrationNumber, string Model, DateTime CreatedAt, VehicleStatus Status);
}
