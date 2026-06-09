namespace LitNovel.Application.DTOs.Chapter
{
    public class UpdateChapterResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Status { get; set; } = default!;
        public DateTime UpdatedAt { get; set; }
    }
}
