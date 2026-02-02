using Microsoft.AspNetCore.Mvc;
using Fleet.Api.Dtos;
using Fleet.Application.Interfaces;
using Fleet.Domain.Entities;

namespace Fleet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleRepository _repo;

        public VehiclesController(IVehicleRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVehicleRequest req)
        {
            var vehicle = new Vehicle(req.RegistrationNumber, req.Model);
            await _repo.AddAsync(vehicle);
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
