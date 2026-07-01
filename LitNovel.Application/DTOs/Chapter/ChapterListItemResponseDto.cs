namespace LitNovel.Application.DTOs.Chapter
{
    public class ChapterListItemResponseDto
    {
        public int Id { get; set; }
        public string Slug { get; set; } = default!;
        public int ChapterNumber { get; set; }
        public string Title { get; set; } = default!;
        public string Status { get; set; } = default!;
        public DateTime? DeletionRequestedAt { get; set; }
        public DateTime? ScheduledHardDeleteAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
