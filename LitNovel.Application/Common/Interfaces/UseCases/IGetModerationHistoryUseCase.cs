using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetModerationHistoryUseCase
    {
        Task<PagedResult<ModerationHistoryItemResponseDto>> ExecuteAsync(int page, int size, CancellationToken ct);
    }
}
