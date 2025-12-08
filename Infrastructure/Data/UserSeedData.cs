using Domain.Entities;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;

public static class UserSeedData
{
    public static async Task InitializeAsync(TMSDbContext context)
    {
        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role { Name = "Admin" },
                new Role { Name = "Manager" },
                new Role { Name = "Employee" }
            );
            await context.SaveChangesAsync();
        }

        if (!context.Users.Any())
        {
            var passwordHasher = new PasswordHasher<User>();

            var admin = new User
            {
                Name = "System Admin",
                Email = "admin@tms.com",
                UserName = "admin",
                IsActive = true,
            };

            admin.Password = passwordHasher.HashPassword(admin, "Admin@123");

            context.Users.Add(admin);
            await context.SaveChangesAsync();

            var adminRole = context.Roles.First(r => r.Name == "Admin");
            context.UserRoles.Add(new UserRole
            {
                UserId = admin.Id,
                RoleId = adminRole.Id
            });

            await context.SaveChangesAsync();
        }
    }
}
