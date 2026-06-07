using LitNovel.Application.DTOs.Volume;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IUpdateVolumeUseCase
    {
        Task<VolumeResponseDto> ExecuteAsync(int id, UpdateVolumeRequestDto request, CancellationToken ct);
    }
}
