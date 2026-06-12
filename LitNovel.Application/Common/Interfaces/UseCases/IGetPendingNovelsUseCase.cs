using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetPendingNovelsUseCase
    {
        Task<PagedResult<PendingNovelListItemResponseDto>> ExecuteAsync(int page, int size, CancellationToken ct);
    }
}
