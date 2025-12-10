using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class UpdateUserDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public List<int> RoleIds { get; set; }
    }
}
