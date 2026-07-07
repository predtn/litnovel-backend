namespace LitNovel.Application.DTOs.Chapter
{
    public class ChapterDetailResponseDto
    {
        public int Id { get; set; }
        public int ChapterNumber { get; set; }
        public string Title { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string Content { get; set; } = default!;
        public string Status { get; set; } = default!;
        public DateTime? ReleaseDate { get; set; }
        public DateTime? DeletionRequestedAt { get; set; }
        public DateTime? ScheduledHardDeleteAt { get; set; }
        public ChapterVolumeResponseDto Volume { get; set; } = default!;
        public ChapterNovelResponseDto Novel { get; set; } = default!;
        public ChapterNavResponseDto? PrevChapter { get; set; }
        public ChapterNavResponseDto? NextChapter { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
