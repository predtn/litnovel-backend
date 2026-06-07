using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        int ExpiresInSeconds { get; }
    }
}
