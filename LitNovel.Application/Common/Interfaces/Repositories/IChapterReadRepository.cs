using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface IChapterReadRepository
    {
        Task<ChapterRead?> GetByUserAndChapterAsync(int userId, int chapterId, CancellationToken ct);
        Task<HashSet<int>> GetReadChapterIdsByNovelAsync(int userId, int novelId, CancellationToken ct);
        Task<List<ChapterRead>> GetByUserAndNovelForDeleteAsync(int userId, int novelId, CancellationToken ct);
        Task<int> CountReadPublishedChaptersByNovelAsync(int userId, int novelId, CancellationToken ct);
        Task<int> CountPublishedChaptersByNovelAsync(int novelId, CancellationToken ct);
        Task AddAsync(ChapterRead chapterRead, CancellationToken ct);
        void DeleteRange(IEnumerable<ChapterRead> chapterReads);
    }
}
