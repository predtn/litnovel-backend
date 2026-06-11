using LitNovel.Domain.Entities;
using LitNovel.Application.DTOs.Volume;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface IVolumeRepository
    {
        Task<List<Volume>> GetByNovelIdAsync(int novelId, CancellationToken ct);
        IQueryable<VolumeResponseDto> QueryByNovelId(int novelId);
        Task<Volume?> GetByIdForUpdateAsync(int volumeId, CancellationToken ct);
        Task<Volume?> GetByIdForDeleteAsync(int volumeId, CancellationToken ct);
        Task<bool> VolumeNumberExistsAsync(int novelId, int volumeNumber, int? excludeVolumeId, CancellationToken ct);
        Task AddAsync(Volume volume, CancellationToken ct);
        void Delete(Volume volume);
    }
}
