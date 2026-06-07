using LitNovel.Application.DTOs.Auth;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IForgotPasswordUseCase
    {
        Task ExecuteAsync(ForgotPasswordRequestDto request, CancellationToken ct);
    }
}
