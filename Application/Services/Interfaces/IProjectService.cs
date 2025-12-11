using Application.DTOs.Project;

namespace Application.Services.Interfaces
{
    public interface IProjectService
    {
        Task CreateProject(CreateProjectDto input);
        Task UpdateProject(int id, CreateProjectDto input);
        Task DeleteProject(int id);
        Task<ProjectDetailsDto> GetProjectById(int id);
        Task<List<ProjectDetailsDto>> GetProjectsList();
        Task AssignProject(AssignProjectDto input);
        Task<List<ProjectDetailsDto>> GetMyProjects();
        Task<List<ProjectTypesDto>> GetProjectTypes();
    }
}
