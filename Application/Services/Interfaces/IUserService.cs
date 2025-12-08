using Application.DTOs;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        Task CreateUser(CreateUpdateUserDto user);
        Task<UserListDto> GetUserById(int id);
        Task UpdateUser(int id, CreateUpdateUserDto user);
        Task DeleteUser(int id);
        Task<List<UserListDto>> GetUsers(UserFilterDto filter);
    }
}
