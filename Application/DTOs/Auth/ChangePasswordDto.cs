namespace Application.DTOs.Auth
{
    public class ChangePasswordDto
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
    }
}
