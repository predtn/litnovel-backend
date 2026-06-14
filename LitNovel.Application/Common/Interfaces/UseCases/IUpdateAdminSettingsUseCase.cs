using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IUpdateAdminSettingsUseCase
    {
        Task<AdminSettingsResponseDto> ExecuteAsync(UpdateAdminSettingsRequestDto request, CancellationToken ct);
    }
}
