using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Comment;
using LitNovel.WebAPI.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LitNovel.WebAPI.Controllers
{
    [ApiController]
    [Route("api/comments")]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICreateCommentReplyUseCase _createCommentReplyUseCase;
        private readonly IUpdateCommentUseCase _updateCommentUseCase;
        private readonly IDeleteCommentUseCase _deleteCommentUseCase;
        private readonly IAddCommentLikeUseCase _addCommentLikeUseCase;
        private readonly IRemoveCommentLikeUseCase _removeCommentLikeUseCase;

        public CommentsController(
            ICreateCommentReplyUseCase createCommentReplyUseCase,
            IUpdateCommentUseCase updateCommentUseCase,
            IDeleteCommentUseCase deleteCommentUseCase,
            IAddCommentLikeUseCase addCommentLikeUseCase,
            IRemoveCommentLikeUseCase removeCommentLikeUseCase)
        {
            _createCommentReplyUseCase = createCommentReplyUseCase;
            _updateCommentUseCase = updateCommentUseCase;
            _deleteCommentUseCase = deleteCommentUseCase;
            _addCommentLikeUseCase = addCommentLikeUseCase;
            _removeCommentLikeUseCase = removeCommentLikeUseCase;
        }

        [HttpPost("{id:int}/replies")]
        public async Task<IActionResult> CreateReply(int id, CreateCommentRequestDto request, CancellationToken ct)
        {
            var result = await _createCommentReplyUseCase.ExecuteAsync(id, request, ct);
            return StatusCode(StatusCodes.Status201Created, new ApiResponse<CommentResponseDto> { Success = true, Data = result });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, CreateCommentRequestDto request, CancellationToken ct)
        {
            var result = await _updateCommentUseCase.ExecuteAsync(id, request, ct);
            return Ok(new ApiResponse<CommentResponseDto> { Success = true, Message = "Comment updated", Data = result });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await _deleteCommentUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<object> { Success = true, Data = null });
        }

        [HttpPost("{id:int}/likes")]
        public async Task<IActionResult> AddLike(int id, CancellationToken ct)
        {
            await _addCommentLikeUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<object> { Success = true, Message = "Comment liked", Data = null });
        }

        [HttpDelete("{id:int}/likes")]
        public async Task<IActionResult> RemoveLike(int id, CancellationToken ct)
        {
            await _removeCommentLikeUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<object> { Success = true, Message = "Comment unliked", Data = null });
        }
    }
}
