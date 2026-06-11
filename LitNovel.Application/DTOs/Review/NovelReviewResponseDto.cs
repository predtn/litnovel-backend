namespace LitNovel.Application.DTOs.Review
{
    public class NovelReviewResponseDto
    {
        public int Id { get; set; }
        public NovelReviewUserResponseDto User { get; set; } = default!;
        public int Rating { get; set; }
        public string? Review { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
