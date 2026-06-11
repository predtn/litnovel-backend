using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Review;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetNovelReviewsUseCase
    {
        Task<PagedResult<NovelReviewResponseDto>> ExecuteAsync(int novelId, int page, int size, CancellationToken ct);
    }
}
