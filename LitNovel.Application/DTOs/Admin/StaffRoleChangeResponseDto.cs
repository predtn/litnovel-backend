namespace LitNovel.Application.DTOs.Admin
{
    public class StaffRoleChangeResponseDto
    {
        public int UserId { get; set; }
        public string Role { get; set; } = default!;
        public DateTime UpdatedAt { get; set; }
    }
}
