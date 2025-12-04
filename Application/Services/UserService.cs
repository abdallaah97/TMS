using Application.DTOs;
using Application.Repositories.Interfaces;
using Application.Services.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepo;
        public UserService(IGenericRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public void CreateUser(CreateUpdateUserDto user)
        {
            var userObj = new User();

            userObj.Name = user.Name;
            userObj.Email = user.Email;
            userObj.UserName = user.UserName;
            userObj.Password = user.Password;

            _userRepo.Insert(userObj);
            _userRepo.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            _userRepo.Delete(id);
            _userRepo.SaveChanges();
        }

        public UserListDto GetUserById(int id)
        {
            var user = _userRepo.GetById(id);
            return user == null ? null : new UserListDto
            {
                Id = user.Id,
                Name = user.Name,
                UserEmail = user.Email,
                UserName = user.UserName
            };
        }


        public List<UserListDto> GetUsers(UserFilterDto filter)
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

        public void UpdateUser(int id, CreateUpdateUserDto user)
        {
            var userObj = _userRepo.GetById(id);

            userObj.UserName = user.UserName;
            userObj.Name = user.Name;
            userObj.Email = user.Email;
            userObj.Password = user.Password;

            _userRepo.Update(userObj);
            _userRepo.SaveChanges();
        }
    }
}
