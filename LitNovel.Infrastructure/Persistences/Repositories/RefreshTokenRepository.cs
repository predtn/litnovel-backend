using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly LitNovelContext _context;

        public RefreshTokenRepository(LitNovelContext context)
        {
            _context = context;
        }

        public Task AddAsync(RefreshToken refreshToken, CancellationToken ct)
        {
            return _context.RefreshTokens.AddAsync(refreshToken, ct).AsTask();
        }

        public Task<RefreshToken?> GetActiveAsync(string token, CancellationToken ct)
        {
            return _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow, ct);
        }

        public void Revoke(RefreshToken refreshToken)
        {
            refreshToken.IsRevoked = true;
        }
    }
}
