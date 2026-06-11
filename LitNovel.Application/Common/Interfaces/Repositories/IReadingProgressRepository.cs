using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Reading;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface IReadingProgressRepository
    {
        Task<PagedResult<ReadingProgressResponseDto>> GetHistoryAsync(int userId, ReadingHistoryQueryDto query, CancellationToken ct);
        Task<ReadingProgress?> GetByUserAndNovelAsync(int userId, int novelId, CancellationToken ct);
        Task AddAsync(ReadingProgress progress, CancellationToken ct);
        void Delete(ReadingProgress progress);
    }
}
