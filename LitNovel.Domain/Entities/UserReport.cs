using LitNovel.Domain.Common;

namespace LitNovel.Domain.Entities
{
    public class UserReport : BaseReport
    {
        public int TargetUserId { get; set; }
        public int? TargetCommentId { get; set; }

        public User TargetUser { get; set; } = default!;
        public CommentChapter? TargetComment { get; set; }
    }
}
