using Fleet.Domain.Enums;

namespace Fleet.Domain.Entities
{
    public class Vehicle
    {
        public Guid Id { get; private set; }
        public string RegistrationNumber { get; private set; } = null!;
        public string Model { get; private set; } = null!;
        public VehicleStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Vehicle() { }

        public Vehicle(string registrationNumber, string model)
        {
            Id = Guid.NewGuid();
            RegistrationNumber = registrationNumber;
            Model = model;
            Status = VehicleStatus.Available;
            CreatedAt = DateTime.UtcNow;
        }

        public void SetStatus(VehicleStatus status) => Status = status;
        public void UpdateModel(string model) => Model = model;
    }
}
