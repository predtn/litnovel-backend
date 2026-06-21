namespace LitNovel.Application.DTOs.Admin
{
    public class AdminBackupDownloadDto
    {
        public string FileName { get; set; } = default!;
        public string ContentType { get; set; } = default!;
        public Stream Content { get; set; } = default!;
    }
}
