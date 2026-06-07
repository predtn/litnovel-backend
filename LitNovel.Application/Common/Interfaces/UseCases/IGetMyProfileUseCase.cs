using LitNovel.Application.DTOs.User;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetMyProfileUseCase
    {
        Task<MyProfileResponseDto> ExecuteAsync(CancellationToken ct);
    }
}
