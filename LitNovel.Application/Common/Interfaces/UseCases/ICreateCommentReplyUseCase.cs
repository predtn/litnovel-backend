using LitNovel.Application.DTOs.Comment;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface ICreateCommentReplyUseCase
    {
        Task<CommentResponseDto> ExecuteAsync(int commentId, CreateCommentRequestDto request, CancellationToken ct);
    }
}
