namespace LitNovel.Application.DTOs.Admin
{
    public class AdminNovelStatusResponseDto
    {
        public int NovelId { get; set; }
        public string Status { get; set; } = default!;
        public DateTime UpdatedAt { get; set; }
    }
}
