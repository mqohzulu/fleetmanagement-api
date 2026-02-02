using System;

namespace Maintenance.Api.Dtos
{
    public record CreateWorkOrderRequest(Guid VehicleId, string Description, DateTime? DueAt = null);
}
