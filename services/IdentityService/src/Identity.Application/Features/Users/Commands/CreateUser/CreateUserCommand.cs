using System;

namespace Identity.Application.Features.Users.Commands.CreateUser
{
    public record CreateUserCommand(string Email, string Name, string Password);
}
