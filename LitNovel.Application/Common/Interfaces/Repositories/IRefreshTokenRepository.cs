using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken, CancellationToken ct);
        Task<RefreshToken?> GetActiveAsync(string token, CancellationToken ct);
        Task<bool> HasOtherActiveTokenForUserAsync(int userId, int excludedTokenId, CancellationToken ct);
        void Revoke(RefreshToken refreshToken);
        Task RevokeAllForUserAsync(int userId, CancellationToken ct);
    }
}
