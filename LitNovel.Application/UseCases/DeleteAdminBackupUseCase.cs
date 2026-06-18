using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;

namespace LitNovel.Application.UseCases
{
    public class DeleteAdminBackupUseCase : IDeleteAdminBackupUseCase
    {
        private readonly IBackupRepository _backupRepository;
        private readonly IBackupFileService _backupFileService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAdminBackupUseCase(
            IBackupRepository backupRepository,
            IBackupFileService backupFileService,
            IUnitOfWork unitOfWork)
        {
            _backupRepository = backupRepository;
            _backupFileService = backupFileService;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(string id, CancellationToken ct)
        {
            var backup = await _backupRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Backup not found");

            if (!string.IsNullOrWhiteSpace(backup.FilePath))
            {
                await _backupFileService.DeleteBackupAsync(backup.FilePath, ct);
            }

            _backupRepository.Delete(backup);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
