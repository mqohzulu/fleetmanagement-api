using Identity.Application.Features.Auth.Dtos;
using Identity.Application.Features.Users.Dtos;

namespace Identity.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<Guid> CreateUserAsync(string email, string name, string password, CancellationToken cancellationToken = default);
        Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<AuthResultDto?> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default);
    }
}
