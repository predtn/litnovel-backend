using System.Text.Json;
using LitNovel.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace LitNovel.Infrastructure.Services
{
    public class BackupFileService : IBackupFileService
    {
        private readonly string _backupDirectory;

        public BackupFileService(IConfiguration configuration)
        {
            _backupDirectory = configuration["Backups:Directory"]
                ?? Path.Combine(AppContext.BaseDirectory, "Backups");
        }

        public async Task<(string FilePath, long SizeBytes)> CreateBackupAsync(string backupId, CancellationToken ct)
        {
            Directory.CreateDirectory(_backupDirectory);

            var filePath = Path.Combine(_backupDirectory, $"{backupId}.json");
            var payload = new
            {
                id = backupId,
                createdAt = DateTime.UtcNow,
                note = "LitNovel metadata backup placeholder. Database restore is intentionally not performed automatically."
            };

            await using var stream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 81920, useAsync: true);
            await JsonSerializer.SerializeAsync(stream, payload, cancellationToken: ct);
            await stream.FlushAsync(ct);

            return (filePath, new FileInfo(filePath).Length);
        }

        public Task<Stream> OpenBackupAsync(string filePath, CancellationToken ct)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Backup file not found.", filePath);
            }

            Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, useAsync: true);
            return Task.FromResult(stream);
        }

        public Task RestoreBackupAsync(string filePath, CancellationToken ct)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Backup file not found.", filePath);
            }

            return Task.CompletedTask;
        }

        public Task DeleteBackupAsync(string filePath, CancellationToken ct)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return Task.CompletedTask;
        }
    }
}
