using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class RestoreAdminBackupUseCase : IRestoreAdminBackupUseCase
    {
        private readonly IBackupRepository _backupRepository;
        private readonly IBackupFileService _backupFileService;
        private readonly IValidator<RestoreAdminBackupRequestDto> _validator;

        public RestoreAdminBackupUseCase(
            IBackupRepository backupRepository,
            IBackupFileService backupFileService,
            IValidator<RestoreAdminBackupRequestDto> validator)
        {
            _backupRepository = backupRepository;
            _backupFileService = backupFileService;
            _validator = validator;
        }

        public async Task<AdminRestoreJobResponseDto> ExecuteAsync(string id, RestoreAdminBackupRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var backup = await _backupRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Backup not found");

            if (backup.Status != BackupStatus.Completed || string.IsNullOrWhiteSpace(backup.FilePath))
            {
                throw new BadRequestException("Backup is not ready for restore");
            }

            try
            {
                await _backupFileService.RestoreBackupAsync(backup.FilePath, ct);
            }
            catch (FileNotFoundException)
            {
                throw new NotFoundException("Backup file not found");
            }

            return new AdminRestoreJobResponseDto
            {
                JobId = $"restore_{DateTime.UtcNow:yyyyMMdd_HHmmss}",
                Status = BackupStatus.InProgress.ToString()
            };
        }
    }
}
