namespace LitNovel.Application.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
        public int ExpiresIn { get; set; }
        public AuthUserResponseDto User { get; set; } = default!;
    }
}
