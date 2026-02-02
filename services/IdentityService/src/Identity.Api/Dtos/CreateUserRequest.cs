using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Dtos
{
    public record CreateUserRequest(
        [property: Required, EmailAddress] string Email,
        [property: Required, MinLength(2)] string Name,
        [property: Required, MinLength(6)] string Password);
}
