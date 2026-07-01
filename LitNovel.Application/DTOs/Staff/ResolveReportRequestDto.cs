namespace LitNovel.Application.DTOs.Staff
{
    public class ResolveReportRequestDto
    {
        /// <summary>Resolve | Reject</summary>
        public string Action { get; set; } = default!;
        public string? ActionTaken { get; set; }
        public string? ResolutionNotes { get; set; }
        /// <summary>
        /// Nếu true và Action là Resolve: tự động xóa Comment vi phạm (UserReport)
        /// hoặc chuyển Chapter về Draft (NovelReport).
        /// </summary>
        public bool TakeDownContent { get; set; }
        
        public bool WarnUser { get; set; }
        public bool BanUser { get; set; }
        public int? TargetUserId { get; set; }
    }
}
