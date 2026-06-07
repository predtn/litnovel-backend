using LitNovel.Application.DTOs.Volume;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface ICreateVolumeUseCase
    {
        Task<VolumeResponseDto> ExecuteAsync(int novelId, CreateVolumeRequestDto request, CancellationToken ct);
    }
}
