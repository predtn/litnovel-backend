namespace LitNovel.Application.DTOs.User
{
    public class PublicProfileResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = default!;
        public string? Avatar { get; set; }
        public string? Bio { get; set; }
        public int Reputation { get; set; }
        public List<BadgeResponseDto> Badges { get; set; } = new();
        public UserStatsResponseDto Stats { get; set; } = new();
        public DateTime JoinedAt { get; set; }
    }
}
