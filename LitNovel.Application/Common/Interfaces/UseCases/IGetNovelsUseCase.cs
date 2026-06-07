using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Novel;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetNovelsUseCase
    {
        Task<PagedResult<NovelListItemResponseDto>> ExecuteAsync(NovelListQueryDto query, CancellationToken ct);
    }
}
