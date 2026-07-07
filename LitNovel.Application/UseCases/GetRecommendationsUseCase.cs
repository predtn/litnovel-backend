using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Recommendation;

namespace LitNovel.Application.UseCases
{
    public class GetRecommendationsUseCase : IGetRecommendationsUseCase
    {
        private const int DefaultLimit = 6;
        private const int MaxLimit = 24;

        private readonly IRecommendationClient _recommendationClient;
        private readonly INovelRepository _novelRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetRecommendationsUseCase(
            IRecommendationClient recommendationClient,
            INovelRepository novelRepository,
            ICurrentUserService currentUserService)
        {
            _recommendationClient = recommendationClient;
            _novelRepository = novelRepository;
            _currentUserService = currentUserService;
        }

        public async Task<RecommendationListResponseDto> ExecuteAsync(int limit, CancellationToken ct)
        {
            var safeLimit = Math.Clamp(limit <= 0 ? DefaultLimit : limit, 1, MaxLimit);
            var recommendations = await _recommendationClient.GetRecommendationsAsync(_currentUserService.UserId, safeLimit, ct);
            if (recommendations == null || recommendations.Items.Count == 0)
            {
                return new RecommendationListResponseDto { Strategy = "unavailable" };
            }

            var orderedIds = recommendations.Items
                .Select(item => item.NovelId)
                .Distinct()
                .Take(safeLimit)
                .ToList();

            var novels = await _novelRepository.GetPublicByIdsAsync(orderedIds, ct);
            var novelById = novels.ToDictionary(novel => novel.Id);

            return new RecommendationListResponseDto
            {
                Strategy = recommendations.Strategy,
                Items = orderedIds
                    .Where(novelById.ContainsKey)
                    .Select(id => novelById[id])
                    .ToList()
            };
        }
    }
}
