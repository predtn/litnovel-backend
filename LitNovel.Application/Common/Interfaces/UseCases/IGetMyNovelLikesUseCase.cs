using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Novel;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetMyNovelLikesUseCase
    {
        Task<PagedResult<NovelListItemResponseDto>> ExecuteAsync(int page, int size, CancellationToken ct);
    }
}
