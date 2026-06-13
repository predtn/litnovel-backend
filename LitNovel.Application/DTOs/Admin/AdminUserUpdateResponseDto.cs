namespace LitNovel.Application.DTOs.Admin
{
    public class AdminUserUpdateResponseDto
    {
        public int UserId { get; set; }
        public string Role { get; set; } = default!;
        public string Status { get; set; } = default!;
        public DateTime UpdatedAt { get; set; }
    }
}
