using System;
using System.Threading.Tasks;
using Identity.Application.Events;

namespace Operations.Api.Consumers
{
    public static class UserCreatedHandler
    {
        public static Task HandleAsync(UserCreatedEvent @event)
        {
            Console.WriteLine($"[OperationsService] UserCreatedEvent received: {@event.UserId} - {@event.Email}");
            // TODO: initialize user-related operations data if needed
            return Task.CompletedTask;
        }
    }
}
