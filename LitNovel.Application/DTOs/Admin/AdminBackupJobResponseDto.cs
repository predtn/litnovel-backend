namespace LitNovel.Application.DTOs.Admin
{
    public class AdminBackupJobResponseDto
    {
        public string JobId { get; set; } = default!;
        public string Status { get; set; } = default!;
        public DateTime StartedAt { get; set; }
    }
}
