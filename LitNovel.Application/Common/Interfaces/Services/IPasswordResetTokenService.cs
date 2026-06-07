namespace LitNovel.Application.Common.Interfaces.Services
{
    public interface IPasswordResetTokenService
    {
        string GenerateToken(int userId, string passwordHash);
        bool TryValidateToken(string token, string passwordHash, out int userId);
    }
}
