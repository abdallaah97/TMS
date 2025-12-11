using Application.DTOs.Department;

namespace Application.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task CreateDepartment(CreateDepartmentDto input);
        Task UpdateDepartment(int id, CreateDepartmentDto input);
        Task DeleteDepartment(int id);
        Task<DepartmentDetailsDto> GetDepartmentById(int id);
        Task<List<DepartmentDetailsDto>> GetDepartmentsList();
    }
}
