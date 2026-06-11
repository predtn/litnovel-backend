namespace LitNovel.Application.DTOs.Comment
{
    public class CommentResponseDto
    {
        public int Id { get; set; }
        public CommentUserResponseDto User { get; set; } = default!;
        public string Content { get; set; } = default!;
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public int? ParentCommentId { get; set; }
        public List<CommentResponseDto> Replies { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }
}
