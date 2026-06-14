using LitNovel.Domain.Enums;

namespace LitNovel.Domain.Entities
{
    public class BackupRecord
    {
        public string Id { get; set; } = default!;
        public long SizeBytes { get; set; }
        public BackupStatus Status { get; set; }
        public string? FilePath { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
