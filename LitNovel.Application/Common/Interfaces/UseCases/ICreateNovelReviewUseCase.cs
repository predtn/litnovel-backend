using LitNovel.Application.DTOs.Review;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface ICreateNovelReviewUseCase
    {
        Task<NovelReviewResponseDto> ExecuteAsync(int novelId, CreateNovelReviewRequestDto request, CancellationToken ct);
    }
}
