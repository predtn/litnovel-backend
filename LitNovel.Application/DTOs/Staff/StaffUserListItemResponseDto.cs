namespace LitNovel.Application.DTOs.Staff
{
    public class StaffUserListItemResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
        public int WarningCount { get; set; }
    }
}
