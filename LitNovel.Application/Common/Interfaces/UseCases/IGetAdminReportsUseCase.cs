using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetAdminReportsUseCase
    {
        Task<PagedResult<AdminReportResponseDto>> ExecuteAsync(AdminReportsQueryDto query, CancellationToken ct);
    }
}
