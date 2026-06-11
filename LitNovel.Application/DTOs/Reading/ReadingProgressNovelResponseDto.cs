namespace LitNovel.Application.DTOs.Reading
{
    public class ReadingProgressNovelResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? CoverImage { get; set; }
        public string Status { get; set; } = default!;
    }
}
