using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Chapter;
using LitNovel.Application.DTOs.Staff;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface IChapterRepository
    {
        Task<PagedResult<ChapterListItemResponseDto>> GetByVolumeIdAsync(int volumeId, ChapterListQueryDto query, CancellationToken ct);
        IQueryable<ChapterListItemResponseDto> QueryByVolumeId(int volumeId);
        Task<Chapter?> GetByIdWithDetailsAsync(int id, CancellationToken ct);
        Task<Chapter?> GetByIdForUpdateAsync(int id, CancellationToken ct);
        Task<Chapter?> GetByIdForDeleteAsync(int id, CancellationToken ct);
        Task<bool> ChapterNumberExistsAsync(int volumeId, int chapterNumber, int? excludeChapterId, CancellationToken ct);
        Task AddAsync(Chapter chapter, CancellationToken ct);
        void Delete(Chapter chapter);
        Task<PagedResult<PendingChapterListItemResponseDto>> GetPendingAsync(int page, int size, CancellationToken ct);
        Task<Chapter?> GetByIdForModerationAsync(int id, CancellationToken ct);
        Task<int> CountPendingAsync(CancellationToken ct);
    }
}
