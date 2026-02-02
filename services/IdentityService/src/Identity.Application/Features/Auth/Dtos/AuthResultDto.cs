using Identity.Application.Features.Users.Dtos;

namespace Identity.Application.Features.Auth.Dtos
{
    public record AuthResultDto(string Token, UserDto User);
}
