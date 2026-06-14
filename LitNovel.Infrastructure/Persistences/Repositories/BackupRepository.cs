using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class BackupRepository : IBackupRepository
    {
        private readonly LitNovelContext _context;

        public BackupRepository(LitNovelContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<AdminBackupResponseDto>> GetAllAsync(CancellationToken ct)
        {
            var backups = await _context.BackupRecords
                .AsNoTracking()
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync(ct);

            return backups
                .Select(b => new AdminBackupResponseDto
                {
                    Id = b.Id,
                    SizeBytes = b.SizeBytes,
                    SizeFormatted = FormatSize(b.SizeBytes),
                    Status = b.Status.ToString(),
                    CreatedAt = b.CreatedAt,
                    DownloadUrl = b.Status == BackupStatus.Completed ? $"/api/admin/backups/{b.Id}/download" : null
                })
                .ToList();
        }

        public Task<BackupRecord?> GetByIdAsync(string id, CancellationToken ct)
        {
            return _context.BackupRecords.FirstOrDefaultAsync(b => b.Id == id, ct);
        }

        public Task AddAsync(BackupRecord backupRecord, CancellationToken ct)
        {
            return _context.BackupRecords.AddAsync(backupRecord, ct).AsTask();
        }

        public void Delete(BackupRecord backupRecord)
        {
            _context.BackupRecords.Remove(backupRecord);
        }

        private static string FormatSize(long bytes)
        {
            string[] units = { "B", "KB", "MB", "GB", "TB" };
            var size = (double)bytes;
            var unitIndex = 0;

            while (size >= 1024 && unitIndex < units.Length - 1)
            {
                size /= 1024;
                unitIndex++;
            }

            return unitIndex == 0 ? $"{bytes} {units[unitIndex]}" : $"{size:0.#} {units[unitIndex]}";
        }
    }
}
