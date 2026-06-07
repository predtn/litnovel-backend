using LitNovel.Application.DTOs.Auth;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IRefreshTokenUseCase
    {
        Task<LoginResponseDto> ExecuteAsync(RefreshTokenRequestDto request, CancellationToken ct);
    }
}
