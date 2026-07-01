namespace LitNovel.Domain.Entities
{
    public class ChapterRead
    {
        public int UserId { get; set; }
        public int NovelId { get; set; }
        public int ChapterId { get; set; }
        public DateTime ReadAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = default!;
        public Novel Novel { get; set; } = default!;
        public Chapter Chapter { get; set; } = default!;
    }
}
