using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Novel;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetMyNovelsUseCase
    {
        Task<PagedResult<MyNovelListItemResponseDto>> ExecuteAsync(MyNovelListQueryDto query, CancellationToken ct);
    }
}
