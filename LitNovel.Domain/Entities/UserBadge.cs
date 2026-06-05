namespace LitNovel.Domain.Entities
{
    public class UserBadge
    {
        public int UserId { get; set; }
        public int BadgeId { get; set; }
        public DateTime EarnedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = default!;
        public Badge Badge { get; set; } = default!;
    }
}
