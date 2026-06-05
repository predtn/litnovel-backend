using LitNovel.Domain.Enums;
using LitNovel.Domain.Entities;

namespace LitNovel.Domain.Common
{
    public abstract class BaseReport : BaseEntity
    {
        public ReportType ReportType { get; set; }
        public string? Description { get; set; }
        public ReportStatus Status { get; set; } = ReportStatus.Pending;
        public string? ActionTaken { get; set; }
        public string? ResolutionNotes { get; set; }
        public int ReporterId { get; set; }
        public int? ProcessedById { get; set; }

        public User Reporter { get; set; } = default!;
        public User? ProcessedBy { get; set; }
    }
}
