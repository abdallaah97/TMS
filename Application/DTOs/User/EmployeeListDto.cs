namespace Application.DTOs.User
{
    public class EmployeeListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? UserEmail { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public List<EmployeeListDto> SubEmployee { get; set; }
    }
}
