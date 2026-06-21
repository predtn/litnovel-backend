using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Staff;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface IUserReportRepository
    {
        Task AddAsync(UserReport report, CancellationToken ct);
        Task<PagedResult<ReportListItemResponseDto>> GetListAsync(string? status, int page, int size, CancellationToken ct);
        Task<UserReport?> GetByIdWithDetailsAsync(int id, CancellationToken ct);
        Task<int> CountPendingAsync(CancellationToken ct);
    }
}
