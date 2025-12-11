using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        [MaxLength(200)]
        [MinLength(3)]
        public string Name { get; set; }
        [MaxLength(200)]
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        public string Password { get; set; }
        [MaxLength(200)]
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public UserType UserType { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<TaskApp> Tasks { get; set; }
    }
}
