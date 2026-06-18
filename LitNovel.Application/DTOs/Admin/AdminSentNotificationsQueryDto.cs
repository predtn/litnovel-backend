namespace LitNovel.Application.DTOs.Admin
{
    public class AdminSentNotificationsQueryDto
    {
        public string? NotificationType { get; set; }
        public int? TargetUserId { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
    }
}
