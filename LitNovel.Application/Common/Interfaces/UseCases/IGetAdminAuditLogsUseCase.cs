using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetAdminAuditLogsUseCase
    {
        Task<PagedResult<AdminAuditLogResponseDto>> ExecuteAsync(AdminAuditLogQueryDto query, CancellationToken ct);
        IQueryable<AdminAuditLogResponseDto> ExecuteQuery();
    }
}
