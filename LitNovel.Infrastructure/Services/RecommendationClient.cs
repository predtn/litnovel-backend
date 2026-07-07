using System.Net.Http.Json;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.DTOs.Recommendation;
using Microsoft.Extensions.Logging;

namespace LitNovel.Infrastructure.Services
{
    public class RecommendationClient : IRecommendationClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RecommendationClient> _logger;

        public RecommendationClient(HttpClient httpClient, ILogger<RecommendationClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<RecommendationServiceResponseDto?> GetRecommendationsAsync(int userId, int limit, CancellationToken ct)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<RecommendationServiceResponseDto>(
                    $"/recommend/{userId}?limit={limit}",
                    ct);
            }
            catch (OperationCanceledException) when (!ct.IsCancellationRequested)
            {
                _logger.LogWarning("Recommendation service timed out for user {UserId}", userId);
                return null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "Recommendation service request failed for user {UserId}", userId);
                return null;
            }
        }
    }
}
