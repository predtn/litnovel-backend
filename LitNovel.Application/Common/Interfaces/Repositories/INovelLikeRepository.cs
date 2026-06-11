using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Novel;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface INovelLikeRepository
    {
        Task<PagedResult<NovelListItemResponseDto>> GetByUserAsync(int userId, int page, int size, CancellationToken ct);
        Task<NovelLike?> GetAsync(int userId, int novelId, CancellationToken ct);
        Task AddAsync(NovelLike like, CancellationToken ct);
        void Delete(NovelLike like);
    }
}
