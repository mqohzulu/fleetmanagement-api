using System;

namespace Fleet.SharedKernel.Domain
{
    public abstract class DomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
