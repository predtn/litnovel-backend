using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class DownloadAdminBackupUseCase : IDownloadAdminBackupUseCase
    {
        private readonly IBackupRepository _backupRepository;
        private readonly IBackupFileService _backupFileService;

        public DownloadAdminBackupUseCase(IBackupRepository backupRepository, IBackupFileService backupFileService)
        {
            _backupRepository = backupRepository;
            _backupFileService = backupFileService;
        }

        public async Task<AdminBackupDownloadDto> ExecuteAsync(string id, CancellationToken ct)
        {
            var backup = await _backupRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Backup not found");

            if (backup.Status != BackupStatus.Completed || string.IsNullOrWhiteSpace(backup.FilePath))
            {
                throw new BadRequestException("Backup is not ready for download");
            }

            try
            {
                var stream = await _backupFileService.OpenBackupAsync(backup.FilePath, ct);
                return new AdminBackupDownloadDto
                {
                    FileName = $"{backup.Id}.json",
                    ContentType = "application/json",
                    Content = stream
                };
            }
            catch (FileNotFoundException)
            {
                throw new NotFoundException("Backup file not found");
            }
        }
    }
}
