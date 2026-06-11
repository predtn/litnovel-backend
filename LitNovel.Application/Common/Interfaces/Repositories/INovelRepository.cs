using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Novel;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface INovelRepository
    {
        Task<PagedResult<NovelListItemResponseDto>> GetListAsync(NovelListQueryDto query, CancellationToken ct);
        Task<PagedResult<MyNovelListItemResponseDto>> GetMyNovelsAsync(int authorId, MyNovelListQueryDto query, CancellationToken ct);
        IQueryable<MyNovelListItemResponseDto> QueryMyNovels(int authorId);
        Task<NovelAnalyticsResponseDto> GetAnalyticsAsync(int id, CancellationToken ct);
        Task<Novel?> GetByIdWithDetailsAsync(int id, CancellationToken ct);
        Task<Novel?> GetByIdForUpdateAsync(int id, CancellationToken ct);
        Task<Novel?> GetByIdForDeleteAsync(int id, CancellationToken ct);
        Task<Novel?> GetByIdWithTagsForUpdateAsync(int id, CancellationToken ct);
        Task<bool> TitleExistsForAuthorAsync(int authorId, string title, int? excludeNovelId, CancellationToken ct);
        Task<bool> SlugExistsAsync(string slug, int? excludeNovelId, CancellationToken ct);
        Task AddAsync(Novel novel, CancellationToken ct);
        void Delete(Novel novel);
    }
}
