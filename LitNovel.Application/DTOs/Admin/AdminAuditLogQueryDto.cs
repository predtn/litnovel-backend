namespace LitNovel.Application.DTOs.Admin
{
    public class AdminAuditLogQueryDto
    {
        public int? ActorId { get; set; }
        public string? EntityType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 50;
    }
}
