using LitNovel.Domain.Common;
using LitNovel.Domain.Enums;

namespace LitNovel.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public int UserId { get; set; }
        public NotificationType NotificationType { get; set; }
        public string? EntityType { get; set; }
        public int? EntityId { get; set; }
        public string Message { get; set; } = default!;
        public bool IsRead { get; set; } = false;

        public User User { get; set; } = default!;
    }
}
