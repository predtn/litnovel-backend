namespace LitNovel.Application.DTOs.Reading
{
    public class ReadingProgressChapterResponseDto
    {
        public int Id { get; set; }
        public int ChapterNumber { get; set; }
        public string Title { get; set; } = default!;
    }
}
