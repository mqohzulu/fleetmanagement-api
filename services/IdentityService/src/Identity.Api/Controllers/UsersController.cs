using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Identity.Api.Dtos;
using Identity.Application.Common.Interfaces;
using Identity.Application.Features.Auth.Dtos;

namespace Identity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] CreateUserRequest req)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var id = await _userService.CreateUserAsync(req.Email, req.Name, req.Password);
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return BadRequest();
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var auth = await _userService.AuthenticateAsync(req.Email, req.Password);
            if (auth == null) return Unauthorized();
            return Ok(auth);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
    }
}
