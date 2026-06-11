using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Review;
using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class NovelRatingRepository : INovelRatingRepository
    {
        private readonly LitNovelContext _context;

        public NovelRatingRepository(LitNovelContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<NovelReviewResponseDto>> GetByNovelAsync(int novelId, int page, int size, CancellationToken ct)
        {
            page = page <= 0 ? 1 : page;
            size = size <= 0 ? 20 : size;

            var ratings = _context.NovelRatings
                .AsNoTracking()
                .Where(r => r.NovelId == novelId)
                .OrderByDescending(r => r.CreatedAt);

            var total = await ratings.CountAsync(ct);
            var items = await ratings
                .Skip((page - 1) * size)
                .Take(size)
                .Select(r => new NovelReviewResponseDto
                {
                    Id = r.Id,
                    User = new NovelReviewUserResponseDto
                    {
                        Id = r.User.Id,
                        Username = r.User.Username,
                        Avatar = r.User.Avatar
                    },
                    Rating = r.Rating,
                    Review = r.Review,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt
                })
                .ToListAsync(ct);

            return new PagedResult<NovelReviewResponseDto>
            {
                Items = items,
                Page = page,
                Size = size,
                TotalElements = total,
                TotalPages = (int)Math.Ceiling(total / (double)size)
            };
        }

        public Task<NovelRating?> GetByUserAndNovelAsync(int userId, int novelId, CancellationToken ct)
        {
            return _context.NovelRatings.FirstOrDefaultAsync(r => r.UserId == userId && r.NovelId == novelId, ct);
        }

        public Task<NovelRating?> GetByIdAsync(int id, CancellationToken ct)
        {
            return _context.NovelRatings
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id, ct);
        }

        public async Task AddAsync(NovelRating rating, CancellationToken ct)
        {
            await _context.NovelRatings.AddAsync(rating, ct);
        }

        public void Delete(NovelRating rating)
        {
            _context.NovelRatings.Remove(rating);
        }
    }
}
