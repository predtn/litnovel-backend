using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface IBackupRepository
    {
        Task<IReadOnlyList<AdminBackupResponseDto>> GetAllAsync(CancellationToken ct);
        Task<BackupRecord?> GetByIdAsync(string id, CancellationToken ct);
        Task AddAsync(BackupRecord backupRecord, CancellationToken ct);
        void Delete(BackupRecord backupRecord);
    }
}
