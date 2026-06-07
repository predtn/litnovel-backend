using LitNovel.Application.DTOs.Auth;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IRegisterUseCase
    {
        Task<RegisterResponseDto> ExecuteAsync(RegisterRequestDto request, CancellationToken ct);
    }
}
