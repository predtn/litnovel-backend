using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Review;
using LitNovel.WebAPI.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LitNovel.WebAPI.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IUpdateNovelReviewUseCase _updateNovelReviewUseCase;
        private readonly IDeleteNovelReviewUseCase _deleteNovelReviewUseCase;

        public ReviewsController(IUpdateNovelReviewUseCase updateNovelReviewUseCase, IDeleteNovelReviewUseCase deleteNovelReviewUseCase)
        {
            _updateNovelReviewUseCase = updateNovelReviewUseCase;
            _deleteNovelReviewUseCase = deleteNovelReviewUseCase;
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateNovelReviewRequestDto request, CancellationToken ct)
        {
            var result = await _updateNovelReviewUseCase.ExecuteAsync(id, request, ct);
            return Ok(new ApiResponse<NovelReviewResponseDto> { Success = true, Message = "Review updated", Data = result });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await _deleteNovelReviewUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<object> { Success = true, Data = null });
        }
    }
}
