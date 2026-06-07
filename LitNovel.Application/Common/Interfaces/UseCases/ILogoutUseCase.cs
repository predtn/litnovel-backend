using LitNovel.Application.DTOs.Auth;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface ILogoutUseCase
    {
        Task ExecuteAsync(LogoutRequestDto request, CancellationToken ct);
    }
}
