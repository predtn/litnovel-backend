using LitNovel.Application.DTOs.Review;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IUpdateNovelReviewUseCase
    {
        Task<NovelReviewResponseDto> ExecuteAsync(int id, UpdateNovelReviewRequestDto request, CancellationToken ct);
    }
}
