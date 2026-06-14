using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly LitNovelContext _context;

        public AuditLogRepository(LitNovelContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<AdminAuditLogResponseDto>> GetAdminAuditLogsAsync(AdminAuditLogQueryDto query, CancellationToken ct)
        {
            var page = query.Page <= 0 ? 1 : query.Page;
            var size = query.Size <= 0 ? 50 : query.Size;

            var auditLogs = _context.AuditLogs.AsNoTracking();

            if (query.ActorId.HasValue)
            {
                auditLogs = auditLogs.Where(a => a.ActorId == query.ActorId.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.EntityType))
            {
                var entityType = query.EntityType.Trim();
                auditLogs = auditLogs.Where(a => a.EntityType == entityType);
            }

            if (query.FromDate.HasValue)
            {
                auditLogs = auditLogs.Where(a => a.CreatedAt >= query.FromDate.Value);
            }

            if (query.ToDate.HasValue)
            {
                auditLogs = auditLogs.Where(a => a.CreatedAt <= query.ToDate.Value);
            }

            var total = await auditLogs.CountAsync(ct);
            var items = await auditLogs
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => new AdminAuditLogResponseDto
                {
                    Id = a.Id,
                    Actor = new AdminUserSummaryResponseDto
                    {
                        Id = a.Actor.Id,
                        Username = a.Actor.Username
                    },
                    Action = a.Action,
                    EntityType = a.EntityType,
                    EntityId = a.EntityId,
                    IpAddress = a.IpAddress,
                    CreatedAt = a.CreatedAt
                })
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(ct);

            return new PagedResult<AdminAuditLogResponseDto>
            {
                Items = items,
                Page = page,
                Size = size,
                TotalElements = total,
                TotalPages = (int)Math.Ceiling(total / (double)size)
            };
        }

        public IQueryable<AdminAuditLogResponseDto> QueryAdminAuditLogs()
        {
            return _context.AuditLogs
                .AsNoTracking()
                .Select(a => new AdminAuditLogResponseDto
                {
                    Id = a.Id,
                    Actor = new AdminUserSummaryResponseDto
                    {
                        Id = a.Actor.Id,
                        Username = a.Actor.Username
                    },
                    Action = a.Action,
                    EntityType = a.EntityType,
                    EntityId = a.EntityId,
                    IpAddress = a.IpAddress,
                    CreatedAt = a.CreatedAt
                });
        }

        public async Task AddAsync(AuditLog auditLog, CancellationToken ct)
        {
            await _context.AuditLogs.AddAsync(auditLog, ct);
        }
    }
}
