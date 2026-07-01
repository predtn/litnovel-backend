namespace LitNovel.Application.DTOs.Comment
{
    public class CommentUserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = default!;
        public string? Avatar { get; set; }
        public List<CommentUserBadgeResponseDto> Badges { get; set; } = new();
    }

    public class CommentUserBadgeResponseDto
    {
        public string Key { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public DateTime EarnedAt { get; set; }
    }
}
