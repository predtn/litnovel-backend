using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class ChapterReadRepository : IChapterReadRepository
    {
        private readonly LitNovelContext _context;

        public ChapterReadRepository(LitNovelContext context)
        {
            _context = context;
        }

        public Task<ChapterRead?> GetByUserAndChapterAsync(int userId, int chapterId, CancellationToken ct)
        {
            return _context.ChapterReads
                .FirstOrDefaultAsync(cr => cr.UserId == userId && cr.ChapterId == chapterId, ct);
        }

        public async Task<HashSet<int>> GetReadChapterIdsByNovelAsync(int userId, int novelId, CancellationToken ct)
        {
            var ids = await _context.ChapterReads
                .AsNoTracking()
                .Where(cr => cr.UserId == userId && cr.NovelId == novelId)
                .Select(cr => cr.ChapterId)
                .ToListAsync(ct);

            return ids.ToHashSet();
        }

        public Task<List<ChapterRead>> GetByUserAndNovelForDeleteAsync(int userId, int novelId, CancellationToken ct)
        {
            return _context.ChapterReads
                .Where(cr => cr.UserId == userId && cr.NovelId == novelId)
                .ToListAsync(ct);
        }

        public Task<int> CountReadPublishedChaptersByNovelAsync(int userId, int novelId, CancellationToken ct)
        {
            return _context.ChapterReads
                .AsNoTracking()
                .Where(cr => cr.UserId == userId
                    && cr.NovelId == novelId
                    && cr.Chapter.Status == ChapterStatus.Published)
                .Select(cr => cr.ChapterId)
                .Distinct()
                .CountAsync(ct);
        }

        public Task<int> CountPublishedChaptersByNovelAsync(int novelId, CancellationToken ct)
        {
            return _context.Chapters
                .AsNoTracking()
                .CountAsync(c => c.Volume.NovelId == novelId && c.Status == ChapterStatus.Published, ct);
        }

        public async Task AddAsync(ChapterRead chapterRead, CancellationToken ct)
        {
            await _context.ChapterReads.AddAsync(chapterRead, ct);
        }

        public void DeleteRange(IEnumerable<ChapterRead> chapterReads)
        {
            _context.ChapterReads.RemoveRange(chapterReads);
        }
    }
}
