using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Chapter;
using LitNovel.Application.DTOs.Comment;
using LitNovel.WebAPI.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LitNovel.WebAPI.Controllers
{
    [ApiController]
    [Route("api/chapters")]
    [Authorize]
    public class ChaptersController : ControllerBase
    {
        private readonly IGetChapterUseCase _getChapterUseCase;
        private readonly IUpdateChapterUseCase _updateChapterUseCase;
        private readonly ISubmitChapterUseCase _submitChapterUseCase;
        private readonly IWithdrawChapterSubmissionUseCase _withdrawChapterSubmissionUseCase;
        private readonly IDeleteChapterUseCase _deleteChapterUseCase;
        private readonly IGetChapterCommentsUseCase _getChapterCommentsUseCase;
        private readonly ICreateChapterCommentUseCase _createChapterCommentUseCase;

        public ChaptersController(
            IGetChapterUseCase getChapterUseCase,
            IUpdateChapterUseCase updateChapterUseCase,
            ISubmitChapterUseCase submitChapterUseCase,
            IWithdrawChapterSubmissionUseCase withdrawChapterSubmissionUseCase,
            IDeleteChapterUseCase deleteChapterUseCase,
            IGetChapterCommentsUseCase getChapterCommentsUseCase,
            ICreateChapterCommentUseCase createChapterCommentUseCase)
        {
            _getChapterUseCase = getChapterUseCase;
            _updateChapterUseCase = updateChapterUseCase;
            _submitChapterUseCase = submitChapterUseCase;
            _withdrawChapterSubmissionUseCase = withdrawChapterSubmissionUseCase;
            _deleteChapterUseCase = deleteChapterUseCase;
            _getChapterCommentsUseCase = getChapterCommentsUseCase;
            _createChapterCommentUseCase = createChapterCommentUseCase;
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await _getChapterUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<ChapterDetailResponseDto> { Success = true, Data = result });
        }

        [HttpGet("{slug}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBySlug(string slug, CancellationToken ct)
        {
            var result = await _getChapterUseCase.ExecuteBySlugAsync(slug, ct);
            return Ok(new ApiResponse<ChapterDetailResponseDto> { Success = true, Data = result });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateChapterRequestDto request, CancellationToken ct)
        {
            var result = await _updateChapterUseCase.ExecuteAsync(id, request, ct);
            return Ok(new ApiResponse<UpdateChapterResponseDto> { Success = true, Message = "Chapter updated successfully", Data = result });
        }

        [HttpPost("{id:int}/submit")]
        public async Task<IActionResult> Submit(int id, CancellationToken ct)
        {
            var result = await _submitChapterUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<SubmitChapterResponseDto> { Success = true, Message = "Chapter submitted for review", Data = result });
        }

        [HttpPost("{id:int}/withdraw")]
        public async Task<IActionResult> WithdrawSubmission(int id, CancellationToken ct)
        {
            var result = await _withdrawChapterSubmissionUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<SubmitChapterResponseDto> { Success = true, Message = "Chapter submission withdrawn", Data = result });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await _deleteChapterUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<object> { Success = true, Data = null });
        }

        [HttpGet("{id:int}/comments")]
        [AllowAnonymous]
        public async Task<IActionResult> GetComments(int id, [FromQuery] int page = 1, [FromQuery] int size = 20, CancellationToken ct = default)
        {
            var result = await _getChapterCommentsUseCase.ExecuteAsync(id, page, size, ct);
            return Ok(new ApiResponse<PagedResult<CommentResponseDto>> { Success = true, Data = result });
        }

        [HttpPost("{id:int}/comments")]
        public async Task<IActionResult> CreateComment(int id, CreateCommentRequestDto request, CancellationToken ct)
        {
            var result = await _createChapterCommentUseCase.ExecuteAsync(id, request, ct);
            return StatusCode(StatusCodes.Status201Created, new ApiResponse<CommentResponseDto> { Success = true, Data = result });
        }
    }
}
