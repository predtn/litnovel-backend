namespace LitNovel.Application.DTOs.Comment
{
    public class CommentUserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = default!;
        public string? Avatar { get; set; }
    }
}
