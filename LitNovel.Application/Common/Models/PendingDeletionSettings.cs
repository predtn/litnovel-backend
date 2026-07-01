namespace LitNovel.Application.Common.Models
{
    public class PendingDeletionSettings
    {
        public int RetentionSeconds { get; set; }
        public int InitialDelaySeconds { get; set; }
        public int CleanupIntervalSeconds { get; set; }
    }
}
