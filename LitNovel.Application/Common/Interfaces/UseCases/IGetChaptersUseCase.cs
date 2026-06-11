using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Chapter;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetChaptersUseCase
    {
        Task<PagedResult<ChapterListItemResponseDto>> ExecuteAsync(int volumeId, ChapterListQueryDto query, CancellationToken ct);
        Task<IQueryable<ChapterListItemResponseDto>> ExecuteQueryAsync(int volumeId, CancellationToken ct);
    }
}
