namespace LitNovel.Application.DTOs.User
{
    public class MyProfileResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Avatar { get; set; }
        public string? Bio { get; set; }
        public string Role { get; set; } = default!;
        public string Status { get; set; } = default!;
        public int Reputation { get; set; }
        public List<BadgeResponseDto> Badges { get; set; } = new();
        public UserStatsResponseDto Stats { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }
}
