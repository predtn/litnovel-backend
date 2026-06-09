namespace LitNovel.Application.DTOs.Chapter
{
    public class ChapterNovelResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Slug { get; set; } = default!;
    }
}
