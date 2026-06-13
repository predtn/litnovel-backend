namespace LitNovel.Application.DTOs.Admin
{
    public class AdminReportsQueryDto
    {
        public string? Type { get; set; }
        public string? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? ProcessedById { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
    }
}
