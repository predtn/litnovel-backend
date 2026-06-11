namespace LitNovel.Application.DTOs.Report
{
    public class CreateNovelReportResponseDto
    {
        public int Id { get; set; }
        public int TargetNovelId { get; set; }
        public int? TargetChapterId { get; set; }
        public string ReportType { get; set; } = default!;
        public string Status { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
