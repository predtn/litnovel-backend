namespace LitNovel.Application.DTOs.Report
{
    public class CreateUserReportResponseDto
    {
        public int Id { get; set; }
        public int TargetUserId { get; set; }
        public string ReportType { get; set; } = default!;
        public string Status { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
