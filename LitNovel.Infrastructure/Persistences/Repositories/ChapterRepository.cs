using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Chapter;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class ChapterRepository : IChapterRepository
    {
        private readonly LitNovelContext _context;

        public ChapterRepository(LitNovelContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ChapterListItemResponseDto>> GetByVolumeIdAsync(int volumeId, ChapterListQueryDto query, CancellationToken ct)
        {
            var page = query.Page <= 0 ? 1 : query.Page;
            var size = query.Size <= 0 ? 50 : query.Size;
            var sort = string.IsNullOrWhiteSpace(query.Sort) ? "chapterNumber" : query.Sort;
            var order = string.IsNullOrWhiteSpace(query.Order) ? "asc" : query.Order;

            var chapters = _context.Chapters
                .AsNoTracking()
                .Where(c => c.VolumeId == volumeId);

            if (Enum.TryParse<ChapterStatus>(query.Status, true, out var status))
            {
                chapters = chapters.Where(c => c.Status == status);
            }

            chapters = sort.Equals("createdAt", StringComparison.OrdinalIgnoreCase)
                ? order.Equals("desc", StringComparison.OrdinalIgnoreCase)
                    ? chapters.OrderByDescending(c => c.CreatedAt)
                    : chapters.OrderBy(c => c.CreatedAt)
                : order.Equals("desc", StringComparison.OrdinalIgnoreCase)
                    ? chapters.OrderByDescending(c => c.ChapterNumber)
                    : chapters.OrderBy(c => c.ChapterNumber);

            var totalElements = await chapters.CountAsync(ct);
            var items = await chapters
                .Skip((page - 1) * size)
                .Take(size)
                .Select(c => new ChapterListItemResponseDto
                {
                    Id = c.Id,
                    ChapterNumber = c.ChapterNumber,
                    Title = c.Title,
                    Status = c.Status.ToString(),
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync(ct);

            return new PagedResult<ChapterListItemResponseDto>
            {
                Items = items,
                Page = page,
                Size = size,
                TotalElements = totalElements,
                TotalPages = (int)Math.Ceiling(totalElements / (double)size)
            };
        }

        public IQueryable<ChapterListItemResponseDto> QueryByVolumeId(int volumeId)
        {
            return _context.Chapters
                .AsNoTracking()
                .Where(c => c.VolumeId == volumeId)
                .Select(c => new ChapterListItemResponseDto
                {
                    Id = c.Id,
                    ChapterNumber = c.ChapterNumber,
                    Title = c.Title,
                    Status = c.Status.ToString(),
                    CreatedAt = c.CreatedAt
                });
        }

        public Task<Chapter?> GetByIdWithDetailsAsync(int id, CancellationToken ct)
        {
            return _context.Chapters
                .AsNoTracking()
                .Include(c => c.Content)
                .Include(c => c.Volume)
                    .ThenInclude(v => v.Novel)
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.Id == id, ct);
        }

        public Task<Chapter?> GetByIdForUpdateAsync(int id, CancellationToken ct)
        {
            return _context.Chapters
                .Include(c => c.Content)
                .Include(c => c.Volume)
                    .ThenInclude(v => v.Novel)
                .FirstOrDefaultAsync(c => c.Id == id, ct);
        }

        public Task<Chapter?> GetByIdForDeleteAsync(int id, CancellationToken ct)
        {
            return _context.Chapters
                .Include(c => c.ChapterProgresses)
                .Include(c => c.Volume)
                    .ThenInclude(v => v.Novel)
                .FirstOrDefaultAsync(c => c.Id == id, ct);
        }

        public Task<bool> ChapterNumberExistsAsync(int volumeId, int chapterNumber, int? excludeChapterId, CancellationToken ct)
        {
            return _context.Chapters.AnyAsync(
                c => c.VolumeId == volumeId
                    && c.ChapterNumber == chapterNumber
                    && (!excludeChapterId.HasValue || c.Id != excludeChapterId.Value),
                ct);
        }

        public async Task AddAsync(Chapter chapter, CancellationToken ct)
        {
            await _context.Chapters.AddAsync(chapter, ct);
        }

        public void Delete(Chapter chapter)
        {
            _context.ReadingProgresses.RemoveRange(chapter.ChapterProgresses);
            _context.Chapters.Remove(chapter);
        }
    }
}
