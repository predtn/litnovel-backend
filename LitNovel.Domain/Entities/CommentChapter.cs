using LitNovel.Domain.Common;

namespace LitNovel.Domain.Entities
{
    public class CommentChapter : BaseEntity
    {
        public string Content { get; set; } = default!;
        public int UserId { get; set; }
        public int ChapterId { get; set; }
        public int LikeCount { get; set; } = 0;
        public int DislikeCount { get; set; } = 0;
        public int? ParentCommentId { get; set; }

        public User User { get; set; } = default!;
        public Chapter Chapter { get; set; } = default!;
        public CommentChapter? ParentComment { get; set; }
        public ICollection<CommentChapter> Replies { get; set; } = new List<CommentChapter>();
        public ICollection<CommentLike> CommentLikes { get; set; } = new List<CommentLike>();
        public ICollection<UserReport> TargetReports { get; set; } = new List<UserReport>();
    }
}
