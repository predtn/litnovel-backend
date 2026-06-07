namespace LitNovel.Application.DTOs.Auth
{
    public class ResetPasswordRequestDto
    {
        public string Token { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
    }
}
