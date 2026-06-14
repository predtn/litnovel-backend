namespace LitNovel.Application.DTOs.Admin
{
    public class AdminChapterStatusResponseDto
    {
        public int ChapterId { get; set; }
        public string Status { get; set; } = default!;
        public DateTime UpdatedAt { get; set; }
    }
}
