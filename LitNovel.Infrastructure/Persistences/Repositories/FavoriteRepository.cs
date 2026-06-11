using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Novel;
using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly LitNovelContext _context;

        public FavoriteRepository(LitNovelContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<NovelListItemResponseDto>> GetByUserAsync(int userId, int page, int size, CancellationToken ct)
        {
            page = page <= 0 ? 1 : page;
            size = size <= 0 ? 20 : size;

            var favorites = _context.Favorites
                .AsNoTracking()
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.CreatedAt);

            var total = await favorites.CountAsync(ct);
            var items = await favorites
                .Skip((page - 1) * size)
                .Take(size)
                .Select(f => new NovelListItemResponseDto
                {
                    Id = f.Novel.Id,
                    Title = f.Novel.Title,
                    Slug = f.Novel.Slug,
                    CoverImage = f.Novel.CoverImage,
                    Author = new NovelAuthorResponseDto
                    {
                        Id = f.Novel.Author.Id,
                        Username = f.Novel.Author.Username,
                        Avatar = f.Novel.Author.Avatar
                    },
                    Category = f.Novel.Category == null ? null : new NovelCategoryResponseDto
                    {
                        Id = f.Novel.Category.Id,
                        Name = f.Novel.Category.Name
                    },
                    Tags = f.Novel.NovelTags
                        .Select(nt => new NovelTagResponseDto { Id = nt.Tag.Id, Name = nt.Tag.Name })
                        .ToList(),
                    Status = f.Novel.Status.ToString(),
                    ViewCount = f.Novel.ViewCount,
                    RatingAverage = f.Novel.NovelRatings.Any() ? f.Novel.NovelRatings.Average(r => r.Rating) : 0,
                    UpdatedAt = f.Novel.UpdatedAt
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

        public Task<Favorite?> GetAsync(int userId, int novelId, CancellationToken ct)
        {
            return _context.Favorites.FirstOrDefaultAsync(f => f.UserId == userId && f.NovelId == novelId, ct);
        }

        public async Task AddAsync(Favorite favorite, CancellationToken ct)
        {
            await _context.Favorites.AddAsync(favorite, ct);
        }

        public void Delete(Favorite favorite)
        {
            _context.Favorites.Remove(favorite);
        }
    }
}
