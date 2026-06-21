using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Staff;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface IModerationLogRepository
    {
        Task AddAsync(ModerationLog log, CancellationToken ct);
        Task<PagedResult<ModerationHistoryItemResponseDto>> GetListAsync(
            int? staffId,
            string? actionType,
            DateTime? fromDate,
            DateTime? toDate,
            int page,
            int size,
            CancellationToken ct);
        Task<List<ModerationHistoryItemResponseDto>> GetRecentAsync(int count, CancellationToken ct);
    }
}
