using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class CreateAdminBackupUseCase : ICreateAdminBackupUseCase
    {
        private readonly IBackupRepository _backupRepository;
        private readonly IBackupFileService _backupFileService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateAdminBackupUseCase(
            IBackupRepository backupRepository,
            IBackupFileService backupFileService,
            IUnitOfWork unitOfWork)
        {
            _backupRepository = backupRepository;
            _backupFileService = backupFileService;
            _unitOfWork = unitOfWork;
        }

        public async Task<AdminBackupJobResponseDto> ExecuteAsync(CancellationToken ct)
        {
            var startedAt = DateTime.UtcNow;
            var backupId = $"backup_{startedAt:yyyyMMdd_HHmmss}";
            var (filePath, sizeBytes) = await _backupFileService.CreateBackupAsync(backupId, ct);

            await _backupRepository.AddAsync(new BackupRecord
            {
                Id = backupId,
                SizeBytes = sizeBytes,
                Status = BackupStatus.Completed,
                FilePath = filePath,
                CreatedAt = startedAt,
                UpdatedAt = DateTime.UtcNow
            }, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return new AdminBackupJobResponseDto
            {
                JobId = backupId,
                Status = BackupStatus.InProgress.ToString(),
                StartedAt = startedAt
            };
        }
    }
}
