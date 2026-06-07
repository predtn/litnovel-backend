using LitNovel.Application.DTOs.User;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IUpdateMyProfileUseCase
    {
        Task<UpdateMyProfileResponseDto> ExecuteAsync(UpdateMyProfileRequestDto request, CancellationToken ct);
    }
}
