namespace LitNovel.Application.DTOs.Staff
{
    public class ModerationHistoryItemResponseDto
    {
        public int Id { get; set; }
        public string Kind { get; set; } = default!;       // "NovelReport" | "UserReport"
        public string Action { get; set; } = default!;     // "Resolved" | "Rejected"
        public string? TargetTitle { get; set; }
        public string? ResolutionNotes { get; set; }
        public DateTime ProcessedAt { get; set; }
    }
}
