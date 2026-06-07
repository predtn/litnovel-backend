namespace LitNovel.Application.DTOs.User
{
    public class ChangePasswordRequestDto
    {
        public string CurrentPassword { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
    }
}
