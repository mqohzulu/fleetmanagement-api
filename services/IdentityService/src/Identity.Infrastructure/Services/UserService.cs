using Identity.Application.Common.Interfaces;
using Identity.Application.Features.Auth.Dtos;
using Identity.Application.Features.Users.Dtos;
using Identity.Domain.Entities;
using Identity.Infrastructure.Repositories;

namespace Identity.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtTokenService _jwt;
        private readonly Fleet.Messaging.Abstractions.IEventBus _eventBus;

        public UserService(IUserRepository repo, IPasswordHasher hasher, IJwtTokenService jwt, Fleet.Messaging.Abstractions.IEventBus eventBus)
        {
            _repo = repo;
            _hasher = hasher;
            _jwt = jwt;
            _eventBus = eventBus;
        }

        public async Task<Guid> CreateUserAsync(string email, string name, string password, CancellationToken cancellationToken = default)
        {
            var hash = _hasher.Hash(password);
            var user = new User(email, name, hash);
            await _repo.AddAsync(user, cancellationToken);

            // publish a UserCreatedEvent for other services
            var evt = new Identity.Application.Events.UserCreatedEvent(user.Id, user.Email, user.Name, user.CreatedAt);
            try
            {
                await _eventBus.PublishAsync(evt);
            }
            catch { }

            return user.Id;
        }

        public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var u = await _repo.GetByIdAsync(id, cancellationToken);
            if (u == null) return null;
            return new UserDto(u.Id, u.Email, u.Name, u.CreatedAt);
        }

        public async Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var u = await _repo.GetByEmailAsync(email, cancellationToken);
            if (u == null) return null;
            return new UserDto(u.Id, u.Email, u.Name, u.CreatedAt);
        }

        public async Task<AuthResultDto?> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var u = await _repo.GetByEmailAsync(email, cancellationToken);
            if (u == null) return null;
            if (!_hasher.Verify(u.PasswordHash, password)) return null;
            var userDto = new UserDto(u.Id, u.Email, u.Name, u.CreatedAt);
            var token = _jwt.GenerateToken(userDto);
            return new AuthResultDto(token, userDto);
        }
    }
}
