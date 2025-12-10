using Application.DTOs;
using Application.DTOs.Auth;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TMS.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto user)
        {
            await _userService.CreateUser(user);
            return Ok();
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUser(id);
            return Ok();
        }

        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound("User not exist in the database");
            }
            return Ok(user);
        }

        [HttpPut("UpdateUser/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] CreateUserDto user)
        {
            _userService.UpdateUser(id, user);
            return Ok();
        }

        [HttpPost("GetUsers")]
        public async Task<IActionResult> GetUsers([FromBody] UserFilterDto filter)
        {
            var responseList = await _userService.GetUsers(filter);
            return Ok(responseList);
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto input)
        {
            await _userService.ChangePassword(input);
            return Ok();
        }

    }
}
