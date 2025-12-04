using Application.DTOs;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        void CreateUser(CreateUpdateUserDto user);
        UserListDto GetUserById(int id);
        void UpdateUser(int id, CreateUpdateUserDto user);
        void DeleteUser(int id);
        List<UserListDto> GetUsers(UserFilterDto filter);
    }
}
