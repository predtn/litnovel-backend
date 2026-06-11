using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Comment;
using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class CommentChapterRepository : ICommentChapterRepository
    {
        private readonly LitNovelContext _context;

        public CommentChapterRepository(LitNovelContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<CommentResponseDto>> GetByChapterAsync(int chapterId, int page, int size, CancellationToken ct)
        {
            page = page <= 0 ? 1 : page;
            size = size <= 0 ? 20 : size;

            var comments = _context.CommentChapters
                .AsNoTracking()
                .Where(c => c.ChapterId == chapterId && c.ParentCommentId == null)
                .OrderByDescending(c => c.CreatedAt);

            var total = await comments.CountAsync(ct);
            var items = await comments
                .Skip((page - 1) * size)
                .Take(size)
                .Select(c => new CommentResponseDto
                {
                    Id = c.Id,
                    User = new CommentUserResponseDto
                    {
                        Id = c.User.Id,
                        Username = c.User.Username,
                        Avatar = c.User.Avatar
                    },
                    Content = c.Content,
                    LikeCount = c.LikeCount,
                    DislikeCount = c.DislikeCount,
                    ParentCommentId = c.ParentCommentId,
                    Replies = c.Replies
                        .OrderBy(r => r.CreatedAt)
                        .Select(r => new CommentResponseDto
                        {
                            Id = r.Id,
                            User = new CommentUserResponseDto
                            {
                                Id = r.User.Id,
                                Username = r.User.Username,
                                Avatar = r.User.Avatar
                            },
                            Content = r.Content,
                            LikeCount = r.LikeCount,
                            DislikeCount = r.DislikeCount,
                            ParentCommentId = r.ParentCommentId,
                            CreatedAt = r.CreatedAt
                        })
                        .ToList(),
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync(ct);

            return new PagedResult<CommentResponseDto>
            {
                Items = items,
                Page = page,
                Size = size,
                TotalElements = total,
                TotalPages = (int)Math.Ceiling(total / (double)size)
            };
        }

        public Task<CommentChapter?> GetByIdAsync(int id, CancellationToken ct)
        {
            return _context.CommentChapters
                .Include(c => c.User)
                .Include(c => c.Replies)
                .FirstOrDefaultAsync(c => c.Id == id, ct);
        }

        public async Task AddAsync(CommentChapter comment, CancellationToken ct)
        {
            await _context.CommentChapters.AddAsync(comment, ct);
        }

        public void Delete(CommentChapter comment)
        {
            _context.CommentChapters.RemoveRange(comment.Replies);
            _context.CommentChapters.Remove(comment);
        }
    }
}
