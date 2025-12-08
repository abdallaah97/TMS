using Application.DTOs;
using Application.Repositories.Interfaces;
using Application.Services.Interfaces;
using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepo;
        public UserService(IGenericRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task CreateUser(CreateUpdateUserDto user)
        {
            var userObj = new User();

            userObj.Name = user.Name;
            userObj.Email = user.Email;
            userObj.UserName = user.UserName;
            userObj.Password = user.Password;

            await _userRepo.Insert(userObj);
            await _userRepo.SaveChanges();
        }

        public async Task DeleteUser(int id)
        {
            await _userRepo.Delete(id);
            await _userRepo.SaveChanges();
        }

        public async Task<UserListDto> GetUserById(int id)
        {
            var user = await _userRepo.GetById(id);
            return user == null ? null : new UserListDto
            {
                Id = user.Id,
                Name = user.Name,
                UserEmail = user.Email,
                UserName = user.UserName
            };
        }


        public async Task<List<UserListDto>> GetUsers(UserFilterDto filter)
        {
            // Queryable
            // Enumerable
            var users = _userRepo.GetAll()
                .Where(x => (!string.IsNullOrEmpty(filter.Name) ? x.Name.Trim().ToLower().Contains(filter.Name.Trim().ToLower()) : true)
                && (!string.IsNullOrEmpty(filter.Email) ? x.Email.Trim().ToLower().Equals(filter.Email.Trim().ToLower()) : true));

            var response = users.Select(x => new UserListDto
            {
                Id = x.Id,
                Name = x.Name,
                UserEmail = x.Email,
                UserName = x.UserName
            });

            var responseList = response.ToList();

            return responseList;
        }

        public async Task UpdateUser(int id, CreateUpdateUserDto user)
        {
            var userObj = await _userRepo.GetById(id);

            userObj.UserName = user.UserName;
            userObj.Name = user.Name;
            userObj.Email = user.Email;
            userObj.Password = user.Password;

            _userRepo.Update(userObj);
            await _userRepo.SaveChanges();
        }
    }
}
