using LitNovel.Domain.Common;

namespace LitNovel.Domain.Entities
{
    public class AuditLog : BaseEntity
    {
        public int ActorId { get; set; }
        public string Action { get; set; } = default!;
        public string EntityType { get; set; } = default!;
        public int EntityId { get; set; }
        public string? IpAddress { get; set; }

        public User Actor { get; set; } = default!;
    }
}
