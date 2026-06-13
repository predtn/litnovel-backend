namespace LitNovel.Application.DTOs.Admin
{
    public class AdminSentNotificationResponseDto
    {
        public int Id { get; set; }
        public string NotificationType { get; set; } = default!;
        public string Message { get; set; } = default!;
        public AdminUserSummaryResponseDto TargetUser { get; set; } = new();
        public bool IsRead { get; set; }
        public DateTime SentAt { get; set; }
    }
}
