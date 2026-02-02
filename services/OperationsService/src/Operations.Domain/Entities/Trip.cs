using System;
using Operations.Domain.Enums;

namespace Operations.Domain.Entities
{
    public class Trip
    {
        public Guid Id { get; private set; }
        public Guid VehicleId { get; private set; }
        public Guid DriverId { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime? EndedAt { get; private set; }
        public TripStatus Status { get; private set; }

        private Trip() { }

        public Trip(Guid vehicleId, Guid driverId, DateTime startedAt)
        {
            Id = Guid.NewGuid();
            VehicleId = vehicleId;
            DriverId = driverId;
            StartedAt = startedAt;
            Status = TripStatus.Started;
        }

        public void EndTrip(DateTime endedAt)
        {
            EndedAt = endedAt;
            Status = TripStatus.Completed;
        }
    }
}