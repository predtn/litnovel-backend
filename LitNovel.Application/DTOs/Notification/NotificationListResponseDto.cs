using LitNovel.Application.Common.Models;

namespace LitNovel.Application.DTOs.Notification
{
    /// <summary>
    /// Wrapper response cho GET /api/notifications — bao gồm unreadCount theo spec.
    /// </summary>
    public class NotificationListResponseDto
    {
        public int UnreadCount { get; set; }
        public List<NotificationResponseDto> Items { get; set; } = [];
        public int Page { get; set; }
        public int Size { get; set; }
        public int TotalElements { get; set; }
        public int TotalPages { get; set; }
    }
}
