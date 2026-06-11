using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Review;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface INovelRatingRepository
    {
        Task<PagedResult<NovelReviewResponseDto>> GetByNovelAsync(int novelId, int page, int size, CancellationToken ct);
        Task<NovelRating?> GetByIdAsync(int id, CancellationToken ct);
        Task<NovelRating?> GetByUserAndNovelAsync(int userId, int novelId, CancellationToken ct);
        Task AddAsync(NovelRating rating, CancellationToken ct);
        void Delete(NovelRating rating);
    }
}
