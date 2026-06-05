namespace LitNovel.Domain.Entities
{
    public class NovelRating
    {
        public int Id { get; set; }
        public int NovelId { get; set; }
        public int UserId { get; set; }
        public byte Rating { get; set; } // 1-5
        public string? Review { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public Novel Novel { get; set; } = default!;
        public User User { get; set; } = default!;
    }
}
