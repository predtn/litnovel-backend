namespace LitNovel.Domain.Entities
{
    public class CommentLike
    {
        public int UserId { get; set; }
        public int CommentChapterId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = default!;
        public CommentChapter CommentChapter { get; set; } = default!;
    }
}
