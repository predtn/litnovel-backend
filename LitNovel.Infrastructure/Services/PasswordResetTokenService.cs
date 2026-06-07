using System.Security.Cryptography;
using System.Text;
using LitNovel.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace LitNovel.Infrastructure.Services
{
    public class PasswordResetTokenService : IPasswordResetTokenService
    {
        private readonly IConfiguration _configuration;

        public PasswordResetTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(int userId, string passwordHash)
        {
            var expiresAt = DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds();
            var payload = $"{userId}:{expiresAt}";
            var signature = Sign($"{payload}:{passwordHash}");
            var raw = $"{payload}:{signature}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(raw));
        }

        public bool TryValidateToken(string token, string passwordHash, out int userId)
        {
            userId = 0;

            try
            {
                var raw = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                var parts = raw.Split(':', 3);
                if (parts.Length != 3 || !int.TryParse(parts[0], out userId) || !long.TryParse(parts[1], out var expiresAt))
                {
                    return false;
                }

                if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() > expiresAt)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(passwordHash))
                {
                    return true;
                }

                var expected = Sign($"{parts[0]}:{parts[1]}:{passwordHash}");
                return CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(expected), Encoding.UTF8.GetBytes(parts[2]));
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private string Sign(string value)
        {
            var key = _configuration["Jwt:Key"] ?? "LitNovel development signing key with at least thirty two chars";
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(value)));
        }
    }
}
