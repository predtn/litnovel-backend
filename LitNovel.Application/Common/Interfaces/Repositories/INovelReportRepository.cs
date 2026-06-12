using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Staff;
using LitNovel.Domain.Common;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface INovelReportRepository
    {
        Task AddAsync(NovelReport report, CancellationToken ct);
        Task<PagedResult<ReportListItemResponseDto>> GetListAsync(string? status, int page, int size, CancellationToken ct);
        Task<NovelReport?> GetByIdWithDetailsAsync(int id, CancellationToken ct);
        Task<int> CountPendingAsync(CancellationToken ct);
    }
}
