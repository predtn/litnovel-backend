using LitNovel.Application.DTOs.User;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetPublicProfileUseCase
    {
        Task<PublicProfileResponseDto> ExecuteAsync(int id, CancellationToken ct);
    }
}
