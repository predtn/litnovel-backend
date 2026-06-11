namespace LitNovel.Application.DTOs.Notification
{
    public class NotificationResponseDto
    {
        public int Id { get; set; }
        public string NotificationType { get; set; } = default!;
        public string? EntityType { get; set; }
        public int? EntityId { get; set; }
        public string Message { get; set; } = default!;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
