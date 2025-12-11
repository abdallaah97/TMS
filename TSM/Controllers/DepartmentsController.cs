using Application.DTOs.Department;
using Application.Services.Interfaces;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TMS.Controllers
{
    [Authorize(Roles = TMSConst.ADMIN_ROLE)]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost("CreateDepartment")]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentDto input)
        {
            await _departmentService.CreateDepartment(input);
            return Ok();
        }

        [HttpPost("UpdateDepartment/{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] CreateDepartmentDto input)
        {
            await _departmentService.UpdateDepartment(id, input);
            return Ok();
        }

        [HttpDelete("DeleteDepartment/{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            await _departmentService.DeleteDepartment(id);
            return Ok();
        }
        [HttpGet("GetDepartmentById/{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var department = await _departmentService.GetDepartmentById(id);
            return Ok(department);
        }

        [Authorize(Roles = $"{TMSConst.MANAGER_ROLE},{TMSConst.ADMIN_ROLE}")]
        [HttpGet("GetDepartmentsList")]
        public async Task<IActionResult> GetDepartmentsList()
        {
            var departments = await _departmentService.GetDepartmentsList();
            return Ok(departments);
        }
    }
}
