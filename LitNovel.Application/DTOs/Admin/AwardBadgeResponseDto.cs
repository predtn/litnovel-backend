namespace LitNovel.Application.DTOs.Admin
{
    public class AwardBadgeResponseDto
    {
        public int UserId { get; set; }
        public int BadgeId { get; set; }
        public string Key { get; set; } = default!;
        public string Name { get; set; } = default!;
        public DateTime EarnedAt { get; set; }
    }
}
