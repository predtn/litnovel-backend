using LitNovel.Application.DTOs.Auth;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IResetPasswordUseCase
    {
        Task ExecuteAsync(ResetPasswordRequestDto request, CancellationToken ct);
    }
}
