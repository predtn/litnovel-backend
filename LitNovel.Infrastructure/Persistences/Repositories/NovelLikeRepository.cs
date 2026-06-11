using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Novel;
using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class NovelLikeRepository : INovelLikeRepository
    {
        private readonly LitNovelContext _context;

        public NovelLikeRepository(LitNovelContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<NovelListItemResponseDto>> GetByUserAsync(int userId, int page, int size, CancellationToken ct)
        {
            page = page <= 0 ? 1 : page;
            size = size <= 0 ? 20 : size;

            var likes = _context.NovelLikes
                .AsNoTracking()
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.CreatedAt);

            var total = await likes.CountAsync(ct);
            var items = await likes
                .Skip((page - 1) * size)
                .Take(size)
                .Select(l => new NovelListItemResponseDto
                {
                    Id = l.Novel.Id,
                    Title = l.Novel.Title,
                    Slug = l.Novel.Slug,
                    CoverImage = l.Novel.CoverImage,
                    Author = new NovelAuthorResponseDto
                    {
                        Id = l.Novel.Author.Id,
                        Username = l.Novel.Author.Username,
                        Avatar = l.Novel.Author.Avatar
                    },
                    Category = l.Novel.Category == null ? null : new NovelCategoryResponseDto
                    {
                        Id = l.Novel.Category.Id,
                        Name = l.Novel.Category.Name
                    },
                    Tags = l.Novel.NovelTags
                        .Select(nt => new NovelTagResponseDto { Id = nt.Tag.Id, Name = nt.Tag.Name })
                        .ToList(),
                    Status = l.Novel.Status.ToString(),
                    ViewCount = l.Novel.ViewCount,
                    RatingAverage = l.Novel.NovelRatings.Any() ? l.Novel.NovelRatings.Average(r => r.Rating) : 0,
                    UpdatedAt = l.Novel.UpdatedAt
                })
                .ToListAsync(ct);

            return new PagedResult<NovelListItemResponseDto>
            {
                Items = items,
                Page = page,
                Size = size,
                TotalElements = total,
                TotalPages = (int)Math.Ceiling(total / (double)size)
            };
        }

        public Task<NovelLike?> GetAsync(int userId, int novelId, CancellationToken ct)
        {
            return _context.NovelLikes.FirstOrDefaultAsync(l => l.UserId == userId && l.NovelId == novelId, ct);
        }

        public Task AddAsync(NovelLike like, CancellationToken ct)
        {
            return _context.NovelLikes.AddAsync(like, ct).AsTask();
        }

        public void Delete(NovelLike like)
        {
            _context.NovelLikes.Remove(like);
        }
    }
}
