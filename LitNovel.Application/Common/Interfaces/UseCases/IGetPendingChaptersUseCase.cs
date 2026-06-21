using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetPendingChaptersUseCase
    {
        Task<PagedResult<PendingChapterListItemResponseDto>> ExecuteAsync(int page, int size, CancellationToken ct);
    }
}
