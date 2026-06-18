using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetAdminBackupsUseCase
    {
        Task<IReadOnlyList<AdminBackupResponseDto>> ExecuteAsync(CancellationToken ct);
    }
}
