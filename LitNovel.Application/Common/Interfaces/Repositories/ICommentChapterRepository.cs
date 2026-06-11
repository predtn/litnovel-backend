using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Comment;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface ICommentChapterRepository
    {
        Task<PagedResult<CommentResponseDto>> GetByChapterAsync(int chapterId, int page, int size, CancellationToken ct);
        Task<CommentChapter?> GetByIdAsync(int id, CancellationToken ct);
        Task AddAsync(CommentChapter comment, CancellationToken ct);
        void Delete(CommentChapter comment);
    }
}
