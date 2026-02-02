using System;

namespace Identity.Application.Features.Users.Dtos
{
    public record UserDto(Guid Id, string Email, string Name, DateTime CreatedAt);
}
