using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Comment;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetChapterCommentsUseCase
    {
        Task<PagedResult<CommentResponseDto>> ExecuteAsync(int chapterId, int page, int size, CancellationToken ct);
    }
}
