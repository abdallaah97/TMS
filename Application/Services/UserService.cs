using Application.DTOs;
using Application.DTOs.Auth;
using Application.Repositories.Interfaces;
using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<UserRole> _userRoleRepo;
        public UserService(IGenericRepository<User> userRepo, IGenericRepository<UserRole> userRoleRepo)
        {
            _userRepo = userRepo;
            _userRoleRepo = userRoleRepo;
        }

        public async Task CreateUser(CreateUserDto user)
        {
            var userObj = new User();

            userObj.Name = user.Name;
            userObj.Email = user.Email;
            userObj.UserName = user.UserName;
            userObj.IsActive = true;

            var passwordHasher = new PasswordHasher<User>();
            userObj.Password = passwordHasher.HashPassword(userObj, user.Password);

            await _userRepo.Insert(userObj);
            await _userRepo.SaveChanges();


            var userRoles = new List<UserRole>();
            foreach (var roleId in user.RoleIds)
            {
                var userRole = new UserRole
                {
                    RoleId = roleId,
                    UserId = userObj.Id
                };
                userRoles.Add(userRole);
            }
            await _userRoleRepo.InsertRange(userRoles);
            await _userRepo.SaveChanges();
        }

        public async Task DeleteUser(int id)
        {
            var user = await _userRepo.GetById(id);
            if (user != null)
            {
                var adminUsers = await _userRepo.GetAll().CountAsync(x => (x.UserRoles.Select(x => x.RoleId).ToList()).Contains(3));
                if (adminUsers <= 1)
                {
                    throw new Exception("This is last Admin user");
                }
                await _userRepo.Delete(user);
                await _userRepo.SaveChanges();
            }
            else
            {
                throw new Exception("User not found");
            }

        }

        public async Task<UserListDto> GetUserById(int id)
        {
            var user = await _userRepo.GetAll()
                .Include(x => x.UserRoles)
                .FirstOrDefaultAsync(x => x.Id == id);
            return user == null ? null : new UserListDto
            {
                Id = user.Id,
                Name = user.Name,
                UserEmail = user.Email,
                UserName = user.UserName,
                IsActive = user.IsActive,
                RolesIds = user.UserRoles.Select(ur => ur.RoleId).ToList()
            };
        }


        public async Task<List<UserListDto>> GetUsers(UserFilterDto filter)
        {
            var users = _userRepo.GetAll()
                .Where(x => (!string.IsNullOrEmpty(filter.Name) ? x.Name.Trim().ToLower().Contains(filter.Name.Trim().ToLower()) : true)
                && (!string.IsNullOrEmpty(filter.Email) ? x.Email.Trim().ToLower().Equals(filter.Email.Trim().ToLower()) : true));

            var response = users.Select(x => new UserListDto
            {
                Id = x.Id,
                Name = x.Name,
                UserEmail = x.Email,
                UserName = x.UserName,
                IsActive = x.IsActive,
            });

            var responseList = response.ToList();

            return responseList;
        }

        public async Task UpdateUser(int id, UpdateUserDto user)
        {
            var userObj = await _userRepo.GetAll()
                .Include(x => x.UserRoles)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (userObj == null)
            {
                throw new Exception("User not found!");
            }

            userObj.UserName = user.UserName;
            userObj.Name = user.Name;
            userObj.Email = user.Email;

            _userRepo.Update(userObj);

            _userRoleRepo.DeleteRange(userObj.UserRoles.ToList());

            var userRoles = new List<UserRole>();
            foreach (var roleId in user.RoleIds)
            {
                var userRole = new UserRole
                {
                    RoleId = roleId,
                    UserId = userObj.Id
                };
                userRoles.Add(userRole);
            }
            await _userRoleRepo.InsertRange(userRoles);

            await _userRepo.SaveChanges();
        }

        public async Task ChangePassword(ChangePasswordDto input)
        {
            var user = await _userRepo.GetById(input.UserId);
            if (user == null)
            {
                throw new Exception("User not found!");
            }

            var passwordHasher = new PasswordHasher<User>();
            user.Password = passwordHasher.HashPassword(user, input.NewPassword);

            _userRepo.Update(user);
            await _userRepo.SaveChanges();
        }
    }
}
