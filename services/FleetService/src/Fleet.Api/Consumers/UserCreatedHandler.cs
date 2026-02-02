using System;
using System.Threading.Tasks;
using Identity.Application.Events;

namespace Fleet.Api.Consumers
{
    public static class UserCreatedHandler
    {
        public static Task HandleAsync(UserCreatedEvent @event)
        {
            Console.WriteLine($"[FleetService] UserCreatedEvent received: {@event.UserId} - {@event.Email}");
            // TODO: implement domain behavior (e.g., create driver record)
            return Task.CompletedTask;
        }
    }
}
