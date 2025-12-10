using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class CreateUserDto : UpdateUserDto
    {
        [Required]
        public string Password { get; set; }
    }
}
