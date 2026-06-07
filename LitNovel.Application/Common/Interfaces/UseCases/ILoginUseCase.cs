using LitNovel.Application.DTOs.Auth;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface ILoginUseCase
    {
        Task<LoginResponseDto> ExecuteAsync(LoginRequestDto request, CancellationToken ct);
    }
}
