namespace LitNovel.Application.DTOs.Notification
{
    public class NotificationQueryDto
    {
        public bool? IsRead { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
    }
}
