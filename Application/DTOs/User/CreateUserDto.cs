using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class CreateUserDto : UpdateUserDto
    {
        [Required]
        public string Password { get; set; }
        public UserType UserType { get; set; }
    }
}
