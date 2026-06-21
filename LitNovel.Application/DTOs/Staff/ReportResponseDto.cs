namespace LitNovel.Application.DTOs.Staff
{
    public class ReportListItemResponseDto
    {
        public int Id { get; set; }
        public string Kind { get; set; } = default!; // "Novel" | "User"
        public string ReportType { get; set; } = default!;
        public string? Description { get; set; }
        public string Status { get; set; } = default!;
        public ReportActorDto? Reporter { get; set; }
        public ReportTargetNovelDto? TargetNovel { get; set; }
        public ReportActorDto? TargetUser { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ReportDetailResponseDto : ReportListItemResponseDto
    {
        public string? ActionTaken { get; set; }
        public string? ResolutionNotes { get; set; }
        public ReportActorDto? ProcessedBy { get; set; }
        public ReportTargetChapterDto? TargetChapter { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class ReportActorDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = default!;
        public string? Avatar { get; set; }
    }

    public class ReportTargetNovelDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Slug { get; set; } = default!;
    }

    public class ReportTargetChapterDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public int ChapterNumber { get; set; }
    }
}
