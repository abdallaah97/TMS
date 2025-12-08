using Application.DTOs;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TMS.Controllers
{
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
        public async Task<IActionResult> CreateUser([FromBody] CreateUpdateUserDto user)
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
        public IActionResult UpdateUser(int id, [FromBody] CreateUpdateUserDto user)
        {
            _userService.UpdateUser(id, user);
            return Ok();
        }

        [Authorize]
        [HttpPost("GetUsers")]
        public async Task<IActionResult> GetUsers([FromBody] UserFilterDto filter)
        {
            var responseList = await _userService.GetUsers(filter);
            return Ok(responseList);
        }

    }
}
