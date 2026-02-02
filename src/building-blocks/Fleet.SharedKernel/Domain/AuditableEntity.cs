using System;

namespace Fleet.SharedKernel.Domain
{
    public abstract class AuditableEntity : Entity
    {
        public string? CreatedBy { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public string? LastModifiedBy { get; protected set; }
        public DateTime? LastModifiedAt { get; protected set; }

        protected void SetCreated(string? user)
        {
            CreatedBy = user;
            CreatedAt = DateTime.UtcNow;
        }

        protected void SetModified(string? user)
        {
            LastModifiedBy = user;
            LastModifiedAt = DateTime.UtcNow;
        }
    }
}
