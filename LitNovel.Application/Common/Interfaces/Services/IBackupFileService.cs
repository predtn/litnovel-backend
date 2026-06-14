namespace LitNovel.Application.Common.Interfaces.Services
{
    public interface IBackupFileService
    {
        Task<(string FilePath, long SizeBytes)> CreateBackupAsync(string backupId, CancellationToken ct);
        Task<Stream> OpenBackupAsync(string filePath, CancellationToken ct);
        Task RestoreBackupAsync(string filePath, CancellationToken ct);
        Task DeleteBackupAsync(string filePath, CancellationToken ct);
    }
}
