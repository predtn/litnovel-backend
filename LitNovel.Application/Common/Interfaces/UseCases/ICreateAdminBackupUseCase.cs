using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface ICreateAdminBackupUseCase
    {
        Task<AdminBackupJobResponseDto> ExecuteAsync(CancellationToken ct);
    }
}
