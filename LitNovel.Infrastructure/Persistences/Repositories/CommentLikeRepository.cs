using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class CommentLikeRepository : ICommentLikeRepository
    {
        private readonly LitNovelContext _context;

        public CommentLikeRepository(LitNovelContext context)
        {
            _context = context;
        }

        public Task<CommentLike?> GetAsync(int userId, int commentId, CancellationToken ct)
        {
            return _context.CommentLikes.FirstOrDefaultAsync(l => l.UserId == userId && l.CommentChapterId == commentId, ct);
        }

        public Task AddAsync(CommentLike like, CancellationToken ct)
        {
            return _context.CommentLikes.AddAsync(like, ct).AsTask();
        }

        public void Delete(CommentLike like)
        {
            _context.CommentLikes.Remove(like);
        }
    }
}
