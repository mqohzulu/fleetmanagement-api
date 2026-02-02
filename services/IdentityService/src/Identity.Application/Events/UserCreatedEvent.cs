using System;

namespace Identity.Application.Events
{
    public record UserCreatedEvent(Guid UserId, string Email, string Name, DateTime CreatedAt);
}
