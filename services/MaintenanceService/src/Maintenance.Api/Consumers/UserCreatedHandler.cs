using System;
using System.Threading.Tasks;
using Identity.Application.Events;

namespace Maintenance.Api.Consumers
{
    public static class UserCreatedHandler
    {
        public static Task HandleAsync(UserCreatedEvent @event)
        {
            Console.WriteLine($"[MaintenanceService] UserCreatedEvent received: {@event.UserId} - {@event.Email}");
            // TODO: allocate maintenance-related defaults for the user
            return Task.CompletedTask;
        }
    }
}
