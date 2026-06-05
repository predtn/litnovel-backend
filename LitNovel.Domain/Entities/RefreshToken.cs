using LitNovel.Domain.Common;

namespace LitNovel.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public int UserId { get; set; }
        public string Token { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; } = false;

        public User User { get; set; } = default!;
    }
}
