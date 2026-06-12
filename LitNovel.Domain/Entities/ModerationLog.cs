using LitNovel.Domain.Common;

namespace LitNovel.Domain.Entities
{
    /// <summary>
    /// Audit log for every moderation action taken by Staff/Admin.
    /// Action values: ApproveNovel | RejectNovel | LockNovel |
    ///                ApproveChapter | RejectChapter | LockChapter |
    ///                ResolveReport | RejectReport | WarnUser
    /// </summary>
    public class ModerationLog : BaseEntity
    {
        public int StaffId { get; set; }
        public string Action { get; set; } = default!;
        public string TargetType { get; set; } = default!;  // "Novel" | "Chapter" | "Report" | "User"
        public int TargetId { get; set; }
        public string? TargetTitle { get; set; }
        public string? Notes { get; set; }
        public DateTime PerformedAt { get; set; }

        public User Staff { get; set; } = default!;
    }
}
