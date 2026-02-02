using Microsoft.AspNetCore.Mvc;
using Maintenance.Api.Dtos;
using Maintenance.Application.Interfaces;
using Maintenance.Domain.Entities;

namespace Maintenance.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkOrdersController : ControllerBase
    {
        private readonly IWorkOrderRepository _repo;

        public WorkOrdersController(IWorkOrderRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWorkOrderRequest req)
        {
            var wo = new WorkOrder(req.VehicleId, req.Description, req.DueAt);
            await _repo.AddAsync(wo);
            var dto = new WorkOrderDto(wo.Id, wo.VehicleId, wo.Description, wo.CreatedAt, wo.DueAt, wo.CompletedAt, wo.Status);
            return CreatedAtAction(nameof(Get), new { id = wo.Id }, dto);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var wo = await _repo.GetByIdAsync(id);
            if (wo == null) return NotFound();
            var dto = new WorkOrderDto(wo.Id, wo.VehicleId, wo.Description, wo.CreatedAt, wo.DueAt, wo.CompletedAt, wo.Status);
            return Ok(dto);
        }

        [HttpPost("{id:guid}/complete")]
        public async Task<IActionResult> Complete(Guid id)
        {
            var wo = await _repo.GetByIdAsync(id);
            if (wo == null) return NotFound();
            wo.Complete(DateTime.UtcNow);
            await _repo.UpdateAsync(wo);
            var dto = new WorkOrderDto(wo.Id, wo.VehicleId, wo.Description, wo.CreatedAt, wo.DueAt, wo.CompletedAt, wo.Status);
            return Ok(dto);
        }
    }
}
