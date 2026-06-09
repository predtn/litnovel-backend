namespace LitNovel.Application.DTOs.Chapter
{
    public class SubmitChapterResponseDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = default!;
        public DateTime UpdatedAt { get; set; }
    }
}
