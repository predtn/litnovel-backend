namespace LitNovel.Application.DTOs.Admin
{
    public class UnbanAdminUserResponseDto
    {
        public int UserId { get; set; }
        public string Status { get; set; } = default!;
        public DateTime UpdatedAt { get; set; }
    }
}
