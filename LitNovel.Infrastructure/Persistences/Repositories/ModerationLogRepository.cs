using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Staff;
using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class ModerationLogRepository : IModerationLogRepository
    {
        private readonly LitNovelContext _context;

        public ModerationLogRepository(LitNovelContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ModerationLog log, CancellationToken ct)
        {
            await _context.ModerationLogs.AddAsync(log, ct);
        }

        public async Task<PagedResult<ModerationHistoryItemResponseDto>> GetListAsync(
            int? staffId,
            string? actionType,
            DateTime? fromDate,
            DateTime? toDate,
            int page,
            int size,
            CancellationToken ct)
        {
            var query = _context.ModerationLogs.AsNoTracking();

            if (staffId.HasValue)
                query = query.Where(l => l.StaffId == staffId.Value);

            if (!string.IsNullOrWhiteSpace(actionType))
                query = query.Where(l => l.Action == actionType);

            if (fromDate.HasValue)
                query = query.Where(l => l.PerformedAt >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(l => l.PerformedAt <= toDate.Value);

            var total = await query.CountAsync(ct);
            var items = await query
                .OrderByDescending(l => l.PerformedAt)
                .Skip((page - 1) * size)
                .Take(size)
                .Select(l => new ModerationHistoryItemResponseDto
                {
                    Id            = l.Id,
                    StaffId       = l.StaffId,
                    StaffUsername = l.Staff.Username,
                    Action        = l.Action,
                    TargetType    = l.TargetType,
                    TargetId      = l.TargetId,
                    TargetTitle   = l.TargetTitle,
                    Notes         = l.Notes,
                    PerformedAt   = l.PerformedAt
                })
                .ToListAsync(ct);

            return new PagedResult<ModerationHistoryItemResponseDto>
            {
                Items         = items,
                Page          = page,
                Size          = size,
                TotalElements = total,
                TotalPages    = (int)Math.Ceiling(total / (double)size)
            };
        }

        public async Task<List<ModerationHistoryItemResponseDto>> GetRecentAsync(int count, CancellationToken ct)
        {
            return await _context.ModerationLogs
                .AsNoTracking()
                .OrderByDescending(l => l.PerformedAt)
                .Take(count)
                .Select(l => new ModerationHistoryItemResponseDto
                {
                    Id            = l.Id,
                    StaffId       = l.StaffId,
                    StaffUsername = l.Staff.Username,
                    Action        = l.Action,
                    TargetType    = l.TargetType,
                    TargetId      = l.TargetId,
                    TargetTitle   = l.TargetTitle,
                    Notes         = l.Notes,
                    PerformedAt   = l.PerformedAt
                })
                .ToListAsync(ct);
        }
    }
}
