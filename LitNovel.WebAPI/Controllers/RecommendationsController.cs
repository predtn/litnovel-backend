using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Recommendation;
using LitNovel.WebAPI.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LitNovel.WebAPI.Controllers
{
    [ApiController]
    [Route("api/recommendations")]
    [Authorize]
    public class RecommendationsController : ControllerBase
    {
        private readonly IGetRecommendationsUseCase _getRecommendationsUseCase;

        public RecommendationsController(IGetRecommendationsUseCase getRecommendationsUseCase)
        {
            _getRecommendationsUseCase = getRecommendationsUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int limit = 6, CancellationToken ct = default)
        {
            var result = await _getRecommendationsUseCase.ExecuteAsync(limit, ct);
            return Ok(new ApiResponse<RecommendationListResponseDto> { Success = true, Data = result });
        }
    }
}
