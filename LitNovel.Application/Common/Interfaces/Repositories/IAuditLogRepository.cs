using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface IAuditLogRepository
    {
        Task<PagedResult<AdminAuditLogResponseDto>> GetAdminAuditLogsAsync(AdminAuditLogQueryDto query, CancellationToken ct);
        IQueryable<AdminAuditLogResponseDto> QueryAdminAuditLogs();
        Task AddAsync(AuditLog auditLog, CancellationToken ct);
    }
}
