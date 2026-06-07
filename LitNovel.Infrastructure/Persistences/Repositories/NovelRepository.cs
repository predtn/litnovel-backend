using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Novel;
using LitNovel.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class NovelRepository : INovelRepository
    {
        private readonly LitNovelContext _context;

        public NovelRepository(LitNovelContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<NovelListItemResponseDto>> GetListAsync(NovelListQueryDto query, CancellationToken ct)
        {
            var novels = _context.Novels.AsNoTracking().AsQueryable();

            if (query.AuthorId.HasValue)
            {
                novels = novels.Where(n => n.AuthorId == query.AuthorId.Value);
            }

            if (Enum.TryParse<NovelStatus>(query.Status, true, out var status))
            {
                novels = novels.Where(n => n.Status == status);
            }

            novels = (query.Sort?.ToLowerInvariant(), query.Order?.ToLowerInvariant()) switch
            {
                ("viewcount", "asc") => novels.OrderBy(n => n.ViewCount),
                ("viewcount", _) => novels.OrderByDescending(n => n.ViewCount),
                ("updatedat", "asc") => novels.OrderBy(n => n.UpdatedAt),
                ("updatedat", _) => novels.OrderByDescending(n => n.UpdatedAt),
                _ => novels.OrderByDescending(n => n.UpdatedAt)
            };

            var total = await novels.CountAsync(ct);
            var items = await novels
                .Skip((query.Page - 1) * query.Size)
                .Take(query.Size)
                .Select(n => new NovelListItemResponseDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Slug = n.Slug,
                    CoverImage = n.CoverImage,
                    Author = new NovelAuthorResponseDto
                    {
                        Id = n.Author.Id,
                        Username = n.Author.Username,
                        Avatar = n.Author.Avatar
                    },
                    Category = n.Category == null ? null : new NovelCategoryResponseDto
                    {
                        Id = n.Category.Id,
                        Name = n.Category.Name
                    },
                    Tags = n.NovelTags
                        .Select(nt => new NovelTagResponseDto { Id = nt.Tag.Id, Name = nt.Tag.Name })
                        .ToList(),
                    Status = n.Status.ToString(),
                    ViewCount = n.ViewCount,
                    RatingAverage = n.NovelRatings.Any() ? n.NovelRatings.Average(r => r.Rating) : 0,
                    UpdatedAt = n.UpdatedAt
                })
                .ToListAsync(ct);

            return new PagedResult<NovelListItemResponseDto>
            {
                Items = items,
                Page = query.Page,
                Size = query.Size,
                TotalElements = total,
                TotalPages = (int)Math.Ceiling(total / (double)query.Size)
            };
        }
    }
}
