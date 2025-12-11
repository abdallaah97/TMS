using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Department
{
    public class CreateDepartmentDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Desc { get; set; }
        [Required]
        public int LimitedEmployees { get; set; }
    }
}
