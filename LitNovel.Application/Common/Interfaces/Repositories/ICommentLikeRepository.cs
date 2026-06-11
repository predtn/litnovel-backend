using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface ICommentLikeRepository
    {
        Task<CommentLike?> GetAsync(int userId, int commentId, CancellationToken ct);
        Task AddAsync(CommentLike like, CancellationToken ct);
        void Delete(CommentLike like);
    }
}
