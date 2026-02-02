using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Dtos
{
    public record LoginRequest(
        [property: Required, EmailAddress] string Email,
        [property: Required, MinLength(6)] string Password);
}
