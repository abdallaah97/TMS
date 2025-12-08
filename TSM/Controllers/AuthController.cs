using Application.DTOs.Auth;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace TMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto input)
        {
            var response = await _authService.LoginAsync(input);
            if (response == null)
            {
                return Unauthorized("Invalid email or password");
            }
            return Ok(response);
        }

    }
}
