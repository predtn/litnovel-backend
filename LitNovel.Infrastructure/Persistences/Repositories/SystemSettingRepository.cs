using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class SystemSettingRepository : ISystemSettingRepository
    {
        private readonly LitNovelContext _context;

        public SystemSettingRepository(LitNovelContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<SystemSetting>> GetAllAsync(CancellationToken ct)
        {
            return await _context.SystemSettings
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task UpsertRangeAsync(IReadOnlyDictionary<string, string> settings, CancellationToken ct)
        {
            var keys = settings.Keys.ToList();
            var existingSettings = await _context.SystemSettings
                .Where(setting => keys.Contains(setting.Key))
                .ToListAsync(ct);

            var existingByKey = existingSettings.ToDictionary(setting => setting.Key);

            foreach (var (key, value) in settings)
            {
                if (existingByKey.TryGetValue(key, out var existingSetting))
                {
                    existingSetting.Value = value;
                    continue;
                }

                await _context.SystemSettings.AddAsync(new SystemSetting
                {
                    Key = key,
                    Value = value
                }, ct);
            }
        }
    }
}
