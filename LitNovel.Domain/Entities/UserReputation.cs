namespace LitNovel.Domain.Entities
{
    public class UserReputation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Score { get; set; } = 0;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = default!;
    }
}
