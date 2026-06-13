using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetAdminBadgesUseCase
    {
        Task<IReadOnlyList<AdminBadgeResponseDto>> ExecuteAsync(CancellationToken ct);
    }
}
