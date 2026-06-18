using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class BadgeRepository : IBadgeRepository
    {
        private readonly LitNovelContext _context;

        public BadgeRepository(LitNovelContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<AdminBadgeResponseDto>> GetAdminBadgesAsync(CancellationToken ct)
        {
            return await _context.Badges
                .AsNoTracking()
                .OrderBy(b => b.Name)
                .Select(b => new AdminBadgeResponseDto
                {
                    Id = b.Id,
                    Key = b.Key,
                    Name = b.Name,
                    Description = b.Description,
                    Icon = b.Icon,
                    Color = b.Color,
                    AwardedCount = b.UserBadges.Count
                })
                .ToListAsync(ct);
        }

        public Task<Badge?> GetByIdAsync(int id, CancellationToken ct)
        {
            return _context.Badges
                .Include(b => b.UserBadges)
                .FirstOrDefaultAsync(b => b.Id == id, ct);
        }

        public Task<bool> KeyExistsAsync(string key, int? excludingId, CancellationToken ct)
        {
            var normalized = key.Trim();
            return _context.Badges
                .AsNoTracking()
                .AnyAsync(b => b.Key == normalized && (!excludingId.HasValue || b.Id != excludingId.Value), ct);
        }

        public Task<bool> UserBadgeExistsAsync(int badgeId, int userId, CancellationToken ct)
        {
            return _context.UserBadges
                .AsNoTracking()
                .AnyAsync(ub => ub.BadgeId == badgeId && ub.UserId == userId, ct);
        }

        public Task AddAsync(Badge badge, CancellationToken ct)
        {
            return _context.Badges.AddAsync(badge, ct).AsTask();
        }

        public Task AddUserBadgeAsync(UserBadge userBadge, CancellationToken ct)
        {
            return _context.UserBadges.AddAsync(userBadge, ct).AsTask();
        }

        public void Delete(Badge badge)
        {
            _context.Badges.Remove(badge);
        }
    }
}
