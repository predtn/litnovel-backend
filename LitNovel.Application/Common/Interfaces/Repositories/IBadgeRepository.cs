using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface IBadgeRepository
    {
        Task<IReadOnlyList<AdminBadgeResponseDto>> GetAdminBadgesAsync(CancellationToken ct);
        Task<Badge?> GetByIdAsync(int id, CancellationToken ct);
        Task<bool> KeyExistsAsync(string key, int? excludingId, CancellationToken ct);
        Task<bool> UserBadgeExistsAsync(int badgeId, int userId, CancellationToken ct);
        Task AddAsync(Badge badge, CancellationToken ct);
        Task AddUserBadgeAsync(UserBadge userBadge, CancellationToken ct);
        void Delete(Badge badge);
    }
}
