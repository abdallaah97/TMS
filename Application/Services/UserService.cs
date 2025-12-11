using Application.DTOs.Auth;
using Application.DTOs.User;
using Application.Repositories.Interfaces;
using Application.Services.Interfaces;
using Domain;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<UserProfile> _userProfileRepo;
        private readonly IGenericRepository<UserRole> _userRoleRepo;
        public UserService(IGenericRepository<User> userRepo, IGenericRepository<UserRole> userRoleRepo, IGenericRepository<UserProfile> userProfileRepo)
        {
            _userRepo = userRepo;
            _userRoleRepo = userRoleRepo;
            _userProfileRepo = userProfileRepo;
        }

        public async Task CreateUser(CreateUserDto user)
        {
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            bool passwordValidate = Regex.IsMatch(user.Password, passwordPattern);
            if (!passwordValidate)
            {
                throw new Exception("Passowrd is weaks");
            }

            string emailPattern = @"^[a-zA-Z0-9._%+\-]+@[a-zA-Z0-9.\-]+\.[A-Za-z]{2,}$";
            bool emailValidate = Regex.IsMatch(user.Email, emailPattern);
            if (!emailValidate)
            {
                throw new Exception("Email is not valid");
            }

            string mobilePattern = @"^(?:\+?962|00962)?0?7[7-9]\d{7}$";


            var userObj = new User();

            userObj.Name = user.Name;
            userObj.Email = user.Email;
            userObj.UserName = user.UserName;
            userObj.UserType = user.UserType;
            userObj.IsActive = true;


            var passwordHasher = new PasswordHasher<User>();
            userObj.Password = passwordHasher.HashPassword(userObj, user.Password);

            await _userRepo.Insert(userObj);
            await _userRepo.SaveChanges();

            if (user.UserType == UserType.Employee)
            {
                await _userProfileRepo.Insert(new UserProfile
                {
                    UserId = userObj.Id,
                    DepartmentId = user.DepartmentId,
                    ManagerId = user.ManagerId
                });

                await _userProfileRepo.SaveChanges();
            }

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

                var userProfile = await _userProfileRepo.GetAll().FirstOrDefaultAsync(x => x.UserId == id);

                await _userProfileRepo.Delete(userProfile);
                await _userProfileRepo.SaveChanges();
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
                .Include(x => x.UserRoles).ThenInclude(x => x.Role)
                .Where(x => (!string.IsNullOrEmpty(filter.Name) ? x.Name.Trim().ToLower().Contains(filter.Name.Trim().ToLower()) : true)
                && (!string.IsNullOrEmpty(filter.Email) ? x.Email.Trim().ToLower().Equals(filter.Email.Trim().ToLower()) : true)
                && (filter.IsManager.HasValue ? (filter.IsManager == true ? x.UserRoles.Any(x => x.Role.Name == TMSConst.MANAGER_ROLE) : !x.UserRoles.Any(x => x.Role.Name == "Manager")) : true)
                );

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


            var userProfile = await _userProfileRepo.GetAll().FirstOrDefaultAsync(x => x.UserId == id);
            userProfile.DepartmentId = user.DepartmentId;
            userProfile.ManagerId = user.ManagerId;


            _userProfileRepo.Update(userProfile);
            await _userProfileRepo.SaveChanges();

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

        public List<EmployeeListDto> GetEmployeeUsers(int? userId)
        {
            var employees = _userProfileRepo.GetAll()
                .Include(x => x.Manager)
                .Include(x => x.User)
                .Where(x => userId.HasValue ? x.ManagerId == userId : x.ManagerId == null)
                .ToList();

            var result = employees.Select(e => new EmployeeListDto
            {
                Id = e.User.Id,
                Name = e.User.Name,
                UserEmail = e.User.Email,
                UserName = e.User.UserName,
                IsActive = e.User.IsActive,
                SubEmployee = GetEmployeeUsers(e.User.Id)
            }).ToList();

            return result;
        }

    }
}
