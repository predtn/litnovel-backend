namespace LitNovel.Application.Common.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendPasswordResetAsync(string email, string token, CancellationToken ct);
    }
}
