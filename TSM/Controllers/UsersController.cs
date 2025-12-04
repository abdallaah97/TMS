using Application.DTOs;
using Application.Services.Interfaces;
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
        public IActionResult CreateUser([FromBody] CreateUpdateUserDto user)
        {
            _userService.CreateUser(user);
            return Ok();
        }

        [HttpDelete("DeleteUser")]
        public IActionResult DeleteUser(int id)
        {
            _userService.DeleteUser(id);
            return Ok();
        }

        [HttpGet("GetUserById/{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userService.GetUserById(id);

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

        [HttpPost("GetUsers")]
        public IActionResult GetUsers([FromBody] UserFilterDto filter)
        {
            var responseList = _userService.GetUsers(filter);
            return Ok(responseList);
        }

    }
}
