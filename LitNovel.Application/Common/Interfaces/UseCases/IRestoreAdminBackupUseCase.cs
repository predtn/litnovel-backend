using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IRestoreAdminBackupUseCase
    {
        Task<AdminRestoreJobResponseDto> ExecuteAsync(string id, RestoreAdminBackupRequestDto request, CancellationToken ct);
    }
}
