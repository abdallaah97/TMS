using Domain.Enums;

namespace Application.DTOs.Project
{
    public class ProjectTypesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProjectTypeEnum code { get; set; }
    }
}
