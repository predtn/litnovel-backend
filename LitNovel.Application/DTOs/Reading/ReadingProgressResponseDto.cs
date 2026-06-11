namespace LitNovel.Application.DTOs.Reading
{
    public class ReadingProgressResponseDto
    {
        public ReadingProgressNovelResponseDto Novel { get; set; } = default!;
        public ReadingProgressChapterResponseDto LastChapter { get; set; } = default!;
        public int ProgressPercentage { get; set; }
        public DateTime LastReadAt { get; set; }
    }
}
