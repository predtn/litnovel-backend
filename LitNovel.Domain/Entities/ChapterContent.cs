namespace LitNovel.Domain.Entities
{
    public class ChapterContent
    {
        public int Id { get; set; }
        public int ChapterId { get; set; }
        public string Content { get; set; } = default!;
        public int Version { get; set; } = 1;

        public Chapter Chapter { get; set; } = default!;
    }
}
