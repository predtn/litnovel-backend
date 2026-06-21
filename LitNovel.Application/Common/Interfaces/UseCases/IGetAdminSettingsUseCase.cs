using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetAdminSettingsUseCase
    {
        Task<AdminSettingsResponseDto> ExecuteAsync(CancellationToken ct);
    }
}
