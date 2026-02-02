namespace Identity.Application.Features.Auth.Commands.Register
{
    public record RegisterUserCommand(string Email, string Name, string Password);
}
