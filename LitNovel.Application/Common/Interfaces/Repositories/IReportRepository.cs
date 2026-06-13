using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface IReportRepository
    {
        Task<PagedResult<AdminReportResponseDto>> GetAdminReportsAsync(AdminReportsQueryDto query, CancellationToken ct);
    }
}
