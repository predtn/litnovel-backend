using LitNovel.Application.DTOs.Recommendation;

namespace LitNovel.Application.Common.Interfaces.Services
{
    public interface IRecommendationClient
    {
        Task<RecommendationServiceResponseDto?> GetRecommendationsAsync(int userId, int limit, CancellationToken ct);
    }
}

