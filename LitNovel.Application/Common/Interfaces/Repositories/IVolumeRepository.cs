using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface IVolumeRepository
    {
        Task<List<Volume>> GetByNovelIdAsync(int novelId, CancellationToken ct);
        Task<Volume?> GetByIdForUpdateAsync(int id, CancellationToken ct);
        Task<bool> VolumeNumberExistsAsync(int novelId, int volumeNumber, int? excludeVolumeId, CancellationToken ct);
        Task AddAsync(Volume volume, CancellationToken ct);
        void Delete(Volume volume);
    }
}
