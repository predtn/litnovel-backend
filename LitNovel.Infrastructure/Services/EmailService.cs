using LitNovel.Application.Common.Interfaces.Services;

namespace LitNovel.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public Task SendPasswordResetAsync(string email, string token, CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }
}
