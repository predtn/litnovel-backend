using LitNovel.Application.DTOs.Comment;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface ICreateChapterCommentUseCase
    {
        Task<CommentResponseDto> ExecuteAsync(int chapterId, CreateCommentRequestDto request, CancellationToken ct);
    }
}
