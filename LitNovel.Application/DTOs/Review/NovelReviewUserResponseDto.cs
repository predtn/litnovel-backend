namespace LitNovel.Application.DTOs.Review
{
    public class NovelReviewUserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = default!;
        public string? Avatar { get; set; }
    }
}
