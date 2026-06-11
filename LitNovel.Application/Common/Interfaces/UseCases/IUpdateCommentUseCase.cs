using LitNovel.Application.DTOs.Comment;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IUpdateCommentUseCase
    {
        Task<CommentResponseDto> ExecuteAsync(int id, CreateCommentRequestDto request, CancellationToken ct);
    }
}
