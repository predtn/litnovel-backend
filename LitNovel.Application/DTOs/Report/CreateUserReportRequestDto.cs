namespace LitNovel.Application.DTOs.Report
{
    public class CreateUserReportRequestDto
    {
        public int TargetUserId { get; set; }
        public int? TargetCommentId { get; set; }
        public string ReportType { get; set; } = default!;
        public string? Description { get; set; }
    }
}
