using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Novel;
using LitNovel.Domain.Entities;
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
            var novels = _context.Novels
                .AsNoTracking()
                .Where(n => n.Status == NovelStatus.Ongoing
                    || n.Status == NovelStatus.Ended
                    || n.Status == NovelStatus.Hiatus
                    || n.Status == NovelStatus.Dropped);

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

        public async Task<PagedResult<MyNovelListItemResponseDto>> GetMyNovelsAsync(int authorId, MyNovelListQueryDto query, CancellationToken ct)
        {
            var novels = _context.Novels
                .AsNoTracking()
                .Where(n => n.AuthorId == authorId);

            if (Enum.TryParse<NovelStatus>(query.Status, true, out var status))
            {
                novels = novels.Where(n => n.Status == status);
            }

            novels = (query.Sort?.ToLowerInvariant(), query.Order?.ToLowerInvariant()) switch
            {
                ("createdat", "asc") => novels.OrderBy(n => n.CreatedAt),
                ("createdat", _) => novels.OrderByDescending(n => n.CreatedAt),
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
                .Select(n => new MyNovelListItemResponseDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Slug = n.Slug,
                    CoverImage = n.CoverImage,
                    Status = n.Status.ToString(),
                    TotalChapters = n.TotalChapters,
                    TotalVolumes = n.TotalVolumes,
                    ViewCount = n.ViewCount,
                    RatingAverage = n.NovelRatings.Any() ? n.NovelRatings.Average(r => r.Rating) : 0,
                    CreatedAt = n.CreatedAt,
                    UpdatedAt = n.UpdatedAt
                })
                .ToListAsync(ct);

            return new PagedResult<MyNovelListItemResponseDto>
            {
                Items = items,
                Page = query.Page,
                Size = query.Size,
                TotalElements = total,
                TotalPages = (int)Math.Ceiling(total / (double)query.Size)
            };
        }

        public IQueryable<MyNovelListItemResponseDto> QueryMyNovels(int authorId)
        {
            return _context.Novels
                .AsNoTracking()
                .Where(n => n.AuthorId == authorId)
                .Select(n => new MyNovelListItemResponseDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Slug = n.Slug,
                    CoverImage = n.CoverImage,
                    Status = n.Status.ToString(),
                    TotalChapters = n.TotalChapters,
                    TotalVolumes = n.TotalVolumes,
                    ViewCount = n.ViewCount,
                    RatingAverage = n.NovelRatings.Any() ? n.NovelRatings.Average(r => r.Rating) : 0,
                    CreatedAt = n.CreatedAt,
                    UpdatedAt = n.UpdatedAt
                });
        }

        public async Task<NovelAnalyticsResponseDto> GetAnalyticsAsync(int id, CancellationToken ct)
        {
            var analytics = await _context.Novels
                .AsNoTracking()
                .Where(n => n.Id == id)
                .Select(n => new NovelAnalyticsResponseDto
                {
                    NovelId = n.Id,
                    ViewCount = n.ViewCount,
                    LikeCount = n.LikeCount,
                    FavoritesCount = n.Favorites.Count,
                    RatingAverage = n.NovelRatings.Any() ? n.NovelRatings.Average(r => r.Rating) : 0,
                    RatingCount = n.NovelRatings.Count,
                    CommentCount = n.Volumes
                        .SelectMany(v => v.Chapters)
                        .SelectMany(c => c.CommentChapters)
                        .Count()
                })
                .FirstAsync(ct);

            var ratingCounts = await _context.NovelRatings
                .AsNoTracking()
                .Where(r => r.NovelId == id)
                .GroupBy(r => r.Rating)
                .Select(g => new { Rating = (int)g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Rating, x => x.Count, ct);

            analytics.RatingDistribution = Enumerable.Range(1, 5)
                .ToDictionary(rating => rating, rating => ratingCounts.GetValueOrDefault(rating));

            analytics.TopChapters = await _context.Chapters
                .AsNoTracking()
                .Where(c => c.Volume.NovelId == id)
                .Select(c => new NovelTopChapterResponseDto
                {
                    ChapterId = c.Id,
                    Title = c.Title,
                    CommentCount = c.CommentChapters.Count
                })
                .OrderByDescending(c => c.CommentCount)
                .ThenBy(c => c.ChapterId)
                .Take(5)
                .ToListAsync(ct);

            return analytics;
        }

        public Task<LitNovel.Domain.Entities.Novel?> GetByIdWithDetailsAsync(int id, CancellationToken ct)
        {
            return _context.Novels
                .AsNoTracking()
                .Include(n => n.Author)
                .Include(n => n.Category)
                .Include(n => n.NovelTags)
                    .ThenInclude(nt => nt.Tag)
                .Include(n => n.NovelRatings)
                .Include(n => n.Volumes)
                    .ThenInclude(v => v.Chapters)
                .AsSplitQuery()
                .FirstOrDefaultAsync(n => n.Id == id, ct);
        }

        public Task<LitNovel.Domain.Entities.Novel?> GetByIdForUpdateAsync(int id, CancellationToken ct)
        {
            return _context.Novels.FirstOrDefaultAsync(n => n.Id == id, ct);
        }

        public Task<LitNovel.Domain.Entities.Novel?> GetByIdForDeleteAsync(int id, CancellationToken ct)
        {
            return _context.Novels
                .Include(n => n.TargetReports)
                .Include(n => n.NovelProgresses)
                .FirstOrDefaultAsync(n => n.Id == id, ct);
        }

        public Task<LitNovel.Domain.Entities.Novel?> GetByIdWithTagsForUpdateAsync(int id, CancellationToken ct)
        {
            return _context.Novels
                .Include(n => n.NovelTags)
                .FirstOrDefaultAsync(n => n.Id == id, ct);
        }

        public Task<bool> TitleExistsForAuthorAsync(int authorId, string title, int? excludeNovelId, CancellationToken ct)
        {
            var normalizedTitle = title.Trim().ToLower();
            return _context.Novels.AnyAsync(
                n => n.AuthorId == authorId
                    && n.Title.ToLower() == normalizedTitle
                    && (!excludeNovelId.HasValue || n.Id != excludeNovelId.Value),
                ct);
        }

        public Task<bool> SlugExistsAsync(string slug, int? excludeNovelId, CancellationToken ct)
        {
            return _context.Novels.AnyAsync(n => n.Slug == slug && (!excludeNovelId.HasValue || n.Id != excludeNovelId.Value), ct);
        }

        public async Task AddAsync(Novel novel, CancellationToken ct)
        {
            await _context.Novels.AddAsync(novel, ct);
        }

        public void Delete(LitNovel.Domain.Entities.Novel novel)
        {
            _context.NovelReports.RemoveRange(novel.TargetReports);
            _context.ReadingProgresses.RemoveRange(novel.NovelProgresses);
            _context.Novels.Remove(novel);
        }
    }
}
