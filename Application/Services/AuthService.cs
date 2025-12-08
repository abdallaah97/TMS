using Application.DTOs.Auth;
using Application.Repositories.Interfaces;
using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IGenericRepository<User> _userRepo;

        public AuthService(IConfiguration config, IGenericRepository<User> userRepo)
        {
            _config = config;
            _userRepo = userRepo;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto input)
        {
            var user = _userRepo.GetAll()
                .FirstOrDefault(u => u.Email.Trim().ToLower() == input.Email.Trim().ToLower());

            if (user == null)
            {
                return null;
            }

            var passwordHasher = new PasswordHasher<User>();
            var passowrdResult = passwordHasher.VerifyHashedPassword(user, user.Password, input.Password);

            if (passowrdResult == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            return new LoginResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                UserName = user.UserName,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                //Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
            };
        }

        public string GenerateAccessToken(User user)
        {
            var jwtSection = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserName", user.UserName),
            };

            /*foreach (var role in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Role.Name));
            }
*/
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = jwtSection["Issuer"],
                Audience = jwtSection["Audience"],
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var random = new byte[64];
            RandomNumberGenerator.Fill(random);
            return Convert.ToBase64String(random);
        }
    }
}
