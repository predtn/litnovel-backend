using LitNovel.Application.Common.Interfaces.Services;

namespace LitNovel.Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        private const int WorkFactor = 12;

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                return false;
            }

            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
