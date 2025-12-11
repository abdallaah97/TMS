using Application.DTOs.Auth;
using Application.DTOs.User;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        Task CreateUser(CreateUserDto user);
        Task<UserListDto> GetUserById(int id);
        Task UpdateUser(int id, UpdateUserDto user);
        Task DeleteUser(int id);
        Task<List<UserListDto>> GetUsers(UserFilterDto filter);
        Task ChangePassword(ChangePasswordDto input);
        List<EmployeeListDto> GetEmployeeUsers(int? userId);
    }
}
