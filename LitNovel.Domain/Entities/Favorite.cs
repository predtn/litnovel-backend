namespace LitNovel.Domain.Entities
{
    public class Favorite
    {
        public int UserId { get; set; }
        public int NovelId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = default!;
        public Novel Novel { get; set; } = default!;
    }
}
