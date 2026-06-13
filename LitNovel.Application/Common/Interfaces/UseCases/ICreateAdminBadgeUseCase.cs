using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface ICreateAdminBadgeUseCase
    {
        Task<AdminBadgeResponseDto> ExecuteAsync(CreateAdminBadgeRequestDto request, CancellationToken ct);
    }
}
