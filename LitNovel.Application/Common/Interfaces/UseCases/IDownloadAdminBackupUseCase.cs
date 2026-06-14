using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IDownloadAdminBackupUseCase
    {
        Task<AdminBackupDownloadDto> ExecuteAsync(string id, CancellationToken ct);
    }
}
