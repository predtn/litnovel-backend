namespace LitNovel.Application.DTOs.Staff
{
    public class ModerationHistoryItemResponseDto
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public string StaffUsername { get; set; } = default!;
        public string Action { get; set; } = default!;     // e.g. "ApproveNovel", "RejectChapter"
        public string TargetType { get; set; } = default!; // "Novel" | "Chapter" | "Report" | "User"
        public int TargetId { get; set; }
        public string? TargetTitle { get; set; }
        public string? Notes { get; set; }
        public DateTime PerformedAt { get; set; }
    }
}
