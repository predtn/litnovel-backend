namespace LitNovel.Application.DTOs.Admin
{
    public class AdminUserWarningResponseDto
    {
        public string Reason { get; set; } = default!;
        public string Severity { get; set; } = default!;
        public AdminUserSummaryResponseDto IssuedBy { get; set; } = new();
        public DateTime IssuedAt { get; set; }
    }
}
