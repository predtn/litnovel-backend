namespace LitNovel.Application.DTOs.Admin
{
    public class AdminUserListItemResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Avatar { get; set; }
        public string Role { get; set; } = default!;
        public string Status { get; set; } = default!;
        public int NovelsCount { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
