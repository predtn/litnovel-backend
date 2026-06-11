namespace LitNovel.Application.DTOs.Report
{
    public class CreateNovelReportRequestDto
    {
        public int TargetNovelId { get; set; }
        public int? TargetChapterId { get; set; }
        public string ReportType { get; set; } = default!;
        public string? Description { get; set; }
    }
}
