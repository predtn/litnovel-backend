namespace LitNovel.Application.DTOs.Chapter
{
    public class CreateChapterRequestDto
    {
        public int ChapterNumber { get; set; }
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public DateTime? ReleaseDate { get; set; }
    }
}
