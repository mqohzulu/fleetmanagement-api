using System;

namespace Identity.Api.Dtos
{
    public record UserDto(Guid Id, string Email, string Name, DateTime CreatedAt);
}
