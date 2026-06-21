using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Staff;
using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class UserWarningRepository : IUserWarningRepository
    {
        private readonly LitNovelContext _context;

        public UserWarningRepository(LitNovelContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UserWarning warning, CancellationToken ct)
        {
            await _context.UserWarnings.AddAsync(warning, ct);
        }

        public async Task<PagedResult<UserWarningResponseDto>> GetByUserIdAsync(
            int userId, int page, int size, CancellationToken ct)
        {
            var query = _context.UserWarnings
                .AsNoTracking()
                .Where(w => w.UserId == userId);

            var total = await query.CountAsync(ct);
            var items = await query
                .OrderByDescending(w => w.CreatedAt)
                .Skip((page - 1) * size)
                .Take(size)
                .Select(w => new UserWarningResponseDto
                {
                    Id              = w.Id,
                    Reason          = w.Reason,
                    Severity        = w.Severity.ToString(),
                    IssuedById      = w.IssuedById,
                    IssuedByUsername = w.IssuedBy.Username,
                    IssuedAt        = w.CreatedAt
                })
                .ToListAsync(ct);

            return new PagedResult<UserWarningResponseDto>
            {
                Items         = items,
                Page          = page,
                Size          = size,
                TotalElements = total,
                TotalPages    = (int)Math.Ceiling(total / (double)size)
            };
        }

        public Task<int> CountByUserIdAsync(int userId, CancellationToken ct)
        {
            return _context.UserWarnings.CountAsync(w => w.UserId == userId, ct);
        }

        public Task<int> CountTotalActiveAsync(CancellationToken ct)
        {
            // Count warnings issued in the last 30 days as "active"
            var cutoff = DateTime.UtcNow.AddDays(-30);
            return _context.UserWarnings.CountAsync(w => w.CreatedAt >= cutoff, ct);
        }
    }
}
