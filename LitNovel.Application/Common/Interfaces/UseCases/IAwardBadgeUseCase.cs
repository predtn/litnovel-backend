using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IAwardBadgeUseCase
    {
        Task<AwardBadgeResponseDto> ExecuteAsync(int id, int userId, CancellationToken ct);
    }
}
