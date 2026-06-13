namespace LitNovel.Application.DTOs.Admin
{
    public class AdminReportResponseDto
    {
        public int Id { get; set; }
        public string ReportType { get; set; } = default!;
        public string TargetType { get; set; } = default!;
        public int TargetId { get; set; }
        public string TargetTitle { get; set; } = default!;
        public AdminUserSummaryResponseDto Reporter { get; set; } = new();
        public string Status { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
