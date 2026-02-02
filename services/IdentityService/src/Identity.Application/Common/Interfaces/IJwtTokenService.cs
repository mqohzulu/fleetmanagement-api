using Identity.Application.Features.Users.Dtos;

namespace Identity.Application.Common.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(UserDto user);
    }
}
