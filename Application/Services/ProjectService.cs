using Application.DTOs.Project;
using Application.Repositories.Interfaces;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IGenericRepository<Project> _projectRepo;
        private readonly IGenericRepository<ProjectType> _projectTypeRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProjectService(IGenericRepository<Project> projectRepo, IHttpContextAccessor httpContextAccessor, IGenericRepository<ProjectType> projectTypeRepo)
        {
            _projectRepo = projectRepo;
            _httpContextAccessor = httpContextAccessor;
            _projectTypeRepo = projectTypeRepo;
        }

        public async Task CreateProject(CreateProjectDto input)
        {
            var isNameExist = await _projectRepo.GetAll()
                .AnyAsync(d => d.Name.ToLower().Trim() == input.Name.ToLower().Trim());

            if (isNameExist)
            {
                throw new Exception("Project name already exists.");
            }

            var project = new Project
            {
                Name = input.Name,
                Desc = input.Desc,
                TypeId = input.TypeId,
                StartDate = input.StartDate,
                EndDate = input.EndDate
            };

            await _projectRepo.Insert(project);
            await _projectRepo.SaveChanges();
        }

        public async Task DeleteProject(int id)
        {
            var project = await _projectRepo.GetAll()
                .Include(x => x.Tasks)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (project != null)
            {
                if (!project.Tasks.Any())
                {
                    await _projectRepo.Delete(project);
                    await _projectRepo.SaveChanges();
                }
                else
                {
                    throw new Exception("Cannot delete project already have a tasks.");
                }
            }
            else
            {
                throw new Exception("Project not found");
            }
        }

        public async Task<ProjectDetailsDto> GetProjectById(int id)
        {
            var project = await _projectRepo.GetById(id);

            if (project == null)
            {
                throw new Exception("Project not found");
            }

            var projectDetails = new ProjectDetailsDto
            {
                Id = project.Id,
                Name = project.Name,
                Desc = project.Desc,
                TypeId = project.TypeId,
                StartDate = project.StartDate,
                EndDate = project.EndDate
            };

            return projectDetails;
        }

        public Task<List<ProjectDetailsDto>> GetProjectsList()
        {
            var projects = _projectRepo.GetAll()
                .Select(project => new ProjectDetailsDto
                {
                    Id = project.Id,
                    Name = project.Name,
                    Desc = project.Desc,
                    TypeId = project.TypeId,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate
                }).ToListAsync();

            return projects;
        }

        public async Task UpdateProject(int id, CreateProjectDto input)
        {
            var project = await _projectRepo.GetById(id);

            if (project == null)
            {
                throw new Exception("Project not found");
            }

            project.Name = input.Name;
            project.Desc = input.Desc;
            project.TypeId = input.TypeId;
            project.StartDate = input.StartDate;
            project.EndDate = input.EndDate;

            _projectRepo.Update(project);
            await _projectRepo.SaveChanges();
        }

        public async Task AssignProject(AssignProjectDto input)
        {
            var project = await _projectRepo.GetById(input.ProjectId);
            if (project == null)
            {
                throw new Exception("Project not found");
            }

            project.ProjectManagerId = input.ManagerId;

            _projectRepo.Update(project);
            await _projectRepo.SaveChanges();
        }

        public Task<List<ProjectDetailsDto>> GetMyProjects()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = Convert.ToInt32(userIdClaim);

            var projects = _projectRepo.GetAll()
                .Where(x => x.ProjectManagerId == userId)
                .Select(project => new ProjectDetailsDto
                {
                    Id = project.Id,
                    Name = project.Name,
                    Desc = project.Desc,
                    TypeId = project.TypeId,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate
                }).ToListAsync();

            return projects;
        }

        public Task<List<ProjectTypesDto>> GetProjectTypes()
        {
            var projectTypes = _projectTypeRepo.GetAll()
                .Select(project => new ProjectTypesDto
                {
                    Id = project.Id,
                    Name = project.Name,
                    code = (ProjectTypeEnum)project.Code
                })
                .ToListAsync();

            return projectTypes;
        }
    }
}
