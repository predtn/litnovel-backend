using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Novel;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface IFavoriteRepository
    {
        Task<PagedResult<NovelListItemResponseDto>> GetByUserAsync(int userId, int page, int size, CancellationToken ct);
        Task<Favorite?> GetAsync(int userId, int novelId, CancellationToken ct);
        Task AddAsync(Favorite favorite, CancellationToken ct);
        void Delete(Favorite favorite);
    }
}
