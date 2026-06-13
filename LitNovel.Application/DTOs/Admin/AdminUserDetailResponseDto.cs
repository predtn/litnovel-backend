using LitNovel.Application.DTOs.User;

namespace LitNovel.Application.DTOs.Admin
{
    public class AdminUserDetailResponseDto
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
        public AdminUserDetailStatsResponseDto Stats { get; set; } = new();
        public List<AdminUserWarningResponseDto> Warnings { get; set; } = new();
        public DateTime JoinedAt { get; set; }
    }
}
