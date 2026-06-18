namespace LitNovel.Application.DTOs.Admin
{
    public class AdminAuditLogResponseDto
    {
        public int Id { get; set; }
        public AdminUserSummaryResponseDto Actor { get; set; } = new();
        public string Action { get; set; } = default!;
        public string EntityType { get; set; } = default!;
        public int EntityId { get; set; }
        public string? IpAddress { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
