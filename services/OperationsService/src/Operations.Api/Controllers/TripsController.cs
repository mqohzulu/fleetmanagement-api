using Microsoft.AspNetCore.Mvc;
using Operations.Api.Dtos;
using Operations.Application.Interfaces;
using Operations.Domain.Entities;

namespace Operations.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly ITripRepository _repo;

        public TripsController(ITripRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTripRequest req)
        {
            var startedAt = req.StartedAt ?? DateTime.UtcNow;
            var trip = new Trip(req.VehicleId, req.DriverId, startedAt);
            await _repo.AddAsync(trip);
            var dto = new TripDto(trip.Id, trip.VehicleId, trip.DriverId, trip.StartedAt, trip.EndedAt, trip.Status);
            return CreatedAtAction(nameof(Get), new { id = trip.Id }, dto);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var trip = await _repo.GetByIdAsync(id);
            if (trip == null) return NotFound();
            var dto = new TripDto(trip.Id, trip.VehicleId, trip.DriverId, trip.StartedAt, trip.EndedAt, trip.Status);
            return Ok(dto);
        }

        [HttpPost("{id:guid}/end")]
        public async Task<IActionResult> End(Guid id)
        {
            var trip = await _repo.GetByIdAsync(id);
            if (trip == null) return NotFound();
            trip.EndTrip(DateTime.UtcNow);
            await _repo.UpdateAsync(trip);
            var dto = new TripDto(trip.Id, trip.VehicleId, trip.DriverId, trip.StartedAt, trip.EndedAt, trip.Status);
            return Ok(dto);
        }
    }
}
