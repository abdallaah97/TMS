using Application.DTOs.Project;
using Application.Services.Interfaces;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TMS.Controllers
{
    [Authorize(Roles = TMSConst.ADMIN_ROLE)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost("CreateProject")]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto input)
        {
            await _projectService.CreateProject(input);
            return Ok();
        }

        [HttpPost("UpdateProject/{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] CreateProjectDto input)
        {
            await _projectService.UpdateProject(id, input);
            return Ok();
        }

        [HttpGet("GetProjectById/{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var project = await _projectService.GetProjectById(id);
            return Ok(project);
        }

        [HttpGet("GetProjectsList")]
        public async Task<IActionResult> GetProjectsList()
        {
            var projects = await _projectService.GetProjectsList();
            return Ok(projects);
        }

        [HttpDelete("DeleteProject/{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            await _projectService.DeleteProject(id);
            return Ok();
        }

        [HttpPost("AssignProject")]
        public async Task<IActionResult> AssignProject([FromBody] AssignProjectDto input)
        {
            await _projectService.AssignProject(input);
            return Ok();
        }

        [Authorize(Roles = TMSConst.MANAGER_ROLE)]
        [HttpGet("GetMyProjects")]
        public async Task<IActionResult> GetMyProjects()
        {
            var projects = await _projectService.GetMyProjects();
            return Ok(projects);
        }

        [HttpGet("GetProjectTypes")]
        public async Task<IActionResult> GetProjectTypes()
        {
            var projectTypes = await _projectService.GetProjectTypes();
            return Ok(projectTypes);
        }
    }
}
