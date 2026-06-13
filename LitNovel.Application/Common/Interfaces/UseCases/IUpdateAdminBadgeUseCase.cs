using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IUpdateAdminBadgeUseCase
    {
        Task<AdminBadgeResponseDto> ExecuteAsync(int id, UpdateAdminBadgeRequestDto request, CancellationToken ct);
    }
}
