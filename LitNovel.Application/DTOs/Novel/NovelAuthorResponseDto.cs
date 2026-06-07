namespace LitNovel.Application.DTOs.Novel
{
    public class NovelAuthorResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = default!;
        public string? Avatar { get; set; }
    }
}
