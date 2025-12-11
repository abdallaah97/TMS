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

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto input)
        {
            await _authService.ResetPassword(input);
            return Ok();
        }

        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            var accessToken = await _authService.RefreshToken(refreshToken);
            return Ok(accessToken);
        }

    }
}
