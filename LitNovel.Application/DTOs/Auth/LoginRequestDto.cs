namespace LitNovel.Application.DTOs.Auth
{
    public class LoginRequestDto
    {
        public string Identifier { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
