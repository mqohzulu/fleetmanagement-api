using Microsoft.AspNetCore.Mvc;
using Fleet.Api.Dtos;
using Fleet.Application.Interfaces;
using Fleet.Domain.Entities;
using Fleet.Application.Events;
using Fleet.Messaging.Abstractions;

namespace Fleet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleRepository _repo;
        private readonly IEventBus _eventBus;

        public VehiclesController(IVehicleRepository repo, IEventBus eventBus)
        {
            _repo = repo;
            _eventBus = eventBus;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVehicleRequest req)
        {
            var vehicle = new Vehicle(req.RegistrationNumber, req.Model);
            await _repo.AddAsync(vehicle);
            await _eventBus.PublishAsync(new VehicleCreatedEvent(
                vehicle.Id,
                vehicle.RegistrationNumber,
                vehicle.Model,
                vehicle.CreatedAt));
            var dto = new VehicleDto(vehicle.Id, vehicle.RegistrationNumber, vehicle.Model, vehicle.CreatedAt, vehicle.Status);
            return CreatedAtAction(nameof(Get), new { id = vehicle.Id }, dto);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var vehicle = await _repo.GetByIdAsync(id);
            if (vehicle == null) return NotFound();
            var dto = new VehicleDto(vehicle.Id, vehicle.RegistrationNumber, vehicle.Model, vehicle.CreatedAt, vehicle.Status);
            return Ok(dto);
        }
    }
}
