using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken, CancellationToken ct);
        Task<RefreshToken?> GetActiveAsync(string token, CancellationToken ct);
        void Revoke(RefreshToken refreshToken);
    }
}
