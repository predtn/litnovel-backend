using LitNovel.Application.DTOs.Volume;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetVolumesUseCase
    {
        Task<List<VolumeResponseDto>> ExecuteAsync(int novelId, CancellationToken ct);
    }
}
