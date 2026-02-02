using System;
using Maintenance.Domain.Enums;

namespace Maintenance.Api.Dtos
{
    public record WorkOrderDto(Guid Id, Guid VehicleId, string Description, DateTime CreatedAt, DateTime? DueAt, DateTime? CompletedAt, WorkOrderStatus Status);
}
