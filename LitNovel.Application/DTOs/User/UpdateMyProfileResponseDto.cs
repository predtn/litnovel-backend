namespace LitNovel.Application.DTOs.User
{
    public class UpdateMyProfileResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = default!;
        public string? Avatar { get; set; }
        public string? Bio { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
