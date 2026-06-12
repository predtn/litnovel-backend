namespace LitNovel.Application.DTOs.Staff
{
    public class PendingChapterListItemResponseDto
    {
        public int Id { get; set; }
        public int ChapterNumber { get; set; }
        public string Title { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public int NovelId { get; set; }
        public string NovelTitle { get; set; } = default!;
        public string NovelSlug { get; set; } = default!;
        public int AuthorId { get; set; }
        public string AuthorUsername { get; set; } = default!;
        public DateTime SubmittedAt { get; set; }
    }
}
