namespace LitNovel.Application.DTOs.Admin
{
    public class SendAdminNotificationRequestDto
    {
        public string NotificationType { get; set; } = default!;
        public string Message { get; set; } = default!;
        public bool? TargetAll { get; set; }
        public int? TargetUserId { get; set; }
    }
}
