using LitNovel.Application.DTOs.Recommendation;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetRecommendationsUseCase
    {
        Task<RecommendationListResponseDto> ExecuteAsync(int limit, CancellationToken ct);
    }
}
