namespace LitNovel.Application.DTOs.Admin
{
    public class BanAdminUserResponseDto
    {
        public int UserId { get; set; }
        public string Status { get; set; } = default!;
        public DateTime BannedAt { get; set; }
    }
}
