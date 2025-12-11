namespace Application.DTOs.User
{
    public class UserFilterDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool? IsManager { get; set; } = null;
    }
}
