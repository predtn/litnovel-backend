using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class VolumeRepository : IVolumeRepository
    {
        private readonly LitNovelContext _context;

        public VolumeRepository(LitNovelContext context)
        {
            _context = context;
        }

        public Task<List<Volume>> GetByNovelIdAsync(int novelId, CancellationToken ct)
        {
            return _context.Volumes
                .AsNoTracking()
                .Include(v => v.Chapters)
                .Where(v => v.NovelId == novelId)
                .OrderBy(v => v.VolumeNumber)
                .ToListAsync(ct);
        }

        public Task<Volume?> GetByIdForUpdateAsync(int id, CancellationToken ct)
        {
            return _context.Volumes
                .Include(v => v.Novel)
                .Include(v => v.Chapters)
                .FirstOrDefaultAsync(v => v.Id == id, ct);
        }

        public Task<bool> VolumeNumberExistsAsync(int novelId, int volumeNumber, int? excludeVolumeId, CancellationToken ct)
        {
            return _context.Volumes.AnyAsync(
                v => v.NovelId == novelId
                    && v.VolumeNumber == volumeNumber
                    && (!excludeVolumeId.HasValue || v.Id != excludeVolumeId.Value),
                ct);
        }

        public async Task AddAsync(Volume volume, CancellationToken ct)
        {
            await _context.Volumes.AddAsync(volume, ct);
        }

        public void Delete(Volume volume)
        {
            _context.Volumes.Remove(volume);
        }
    }
}
