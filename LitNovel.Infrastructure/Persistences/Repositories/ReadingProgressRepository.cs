using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Reading;
using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class ReadingProgressRepository : IReadingProgressRepository
    {
        private readonly LitNovelContext _context;

        public ReadingProgressRepository(LitNovelContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ReadingProgressResponseDto>> GetHistoryAsync(int userId, ReadingHistoryQueryDto query, CancellationToken ct)
        {
            var page = query.Page <= 0 ? 1 : query.Page;
            var size = query.Size <= 0 ? 20 : query.Size;

            var progresses = _context.ReadingProgresses
                .AsNoTracking()
                .Where(rp => rp.UserId == userId);

            if (query.Filter?.Equals("in-progress", StringComparison.OrdinalIgnoreCase) == true)
            {
                progresses = progresses.Where(rp => rp.ProgressPercentage < 100);
            }
            else if (query.Filter?.Equals("completed", StringComparison.OrdinalIgnoreCase) == true)
            {
                progresses = progresses.Where(rp => rp.ProgressPercentage >= 100);
            }

            var total = await progresses.CountAsync(ct);
            var items = await progresses
                .OrderByDescending(rp => rp.LastReadAt)
                .Skip((page - 1) * size)
                .Take(size)
                .Select(rp => new ReadingProgressResponseDto
                {
                    Novel = new ReadingProgressNovelResponseDto
                    {
                        Id = rp.Novel.Id,
                        Title = rp.Novel.Title,
                        Slug = rp.Novel.Slug,
                        CoverImage = rp.Novel.CoverImage,
                        Status = rp.Novel.Status.ToString()
                    },
                    LastChapter = new ReadingProgressChapterResponseDto
                    {
                        Id = rp.Chapter.Id,
                        ChapterNumber = rp.Chapter.ChapterNumber,
                        Title = rp.Chapter.Title
                    },
                    ProgressPercentage = rp.ProgressPercentage,
                    LastReadAt = rp.LastReadAt
                })
                .ToListAsync(ct);

            return new PagedResult<ReadingProgressResponseDto>
            {
                Items = items,
                Page = page,
                Size = size,
                TotalElements = total,
                TotalPages = (int)Math.Ceiling(total / (double)size)
            };
        }

        public Task<ReadingProgress?> GetByUserAndNovelAsync(int userId, int novelId, CancellationToken ct)
        {
            return _context.ReadingProgresses.FirstOrDefaultAsync(rp => rp.UserId == userId && rp.NovelId == novelId, ct);
        }

        public async Task AddAsync(ReadingProgress progress, CancellationToken ct)
        {
            await _context.ReadingProgresses.AddAsync(progress, ct);
        }

        public void Delete(ReadingProgress progress)
        {
            _context.ReadingProgresses.Remove(progress);
        }
    }
}
