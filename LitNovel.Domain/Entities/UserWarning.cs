using LitNovel.Domain.Common;
using LitNovel.Domain.Enums;

namespace LitNovel.Domain.Entities
{
    /// <summary>Formal warning issued by Staff/Admin to a User.</summary>
    public class UserWarning : BaseEntity
    {
        public int UserId { get; set; }
        public int IssuedById { get; set; }
        public string Reason { get; set; } = default!;
        public WarningSeverity Severity { get; set; } = WarningSeverity.Minor;

        public User User { get; set; } = default!;
        public User IssuedBy { get; set; } = default!;
    }
}
