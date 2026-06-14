using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.UseCases
{
    public class GetAdminBackupsUseCase : IGetAdminBackupsUseCase
    {
        private readonly IBackupRepository _backupRepository;

        public GetAdminBackupsUseCase(IBackupRepository backupRepository)
        {
            _backupRepository = backupRepository;
        }

        public Task<IReadOnlyList<AdminBackupResponseDto>> ExecuteAsync(CancellationToken ct)
        {
            return _backupRepository.GetAllAsync(ct);
        }
    }
}
