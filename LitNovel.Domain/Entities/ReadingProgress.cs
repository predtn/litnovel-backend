namespace LitNovel.Domain.Entities
{
    public class ReadingProgress
    {
        public int UserId { get; set; }
        public int NovelId { get; set; }
        public int ChapterId { get; set; }
        public byte ProgressPercentage { get; set; } // 0-100
        public DateTime LastReadAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = default!;
        public Novel Novel { get; set; } = default!;
        public Chapter Chapter { get; set; } = default!;
    }
}
