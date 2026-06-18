namespace LitNovel.Application.DTOs.Admin
{
    public class AdminBackupResponseDto
    {
        public string Id { get; set; } = default!;
        public long SizeBytes { get; set; }
        public string SizeFormatted { get; set; } = default!;
        public string Status { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public string? DownloadUrl { get; set; }
    }
}
