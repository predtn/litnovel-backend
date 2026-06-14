using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface ISystemSettingRepository
    {
        Task<IReadOnlyList<SystemSetting>> GetAllAsync(CancellationToken ct);
        Task UpsertRangeAsync(IReadOnlyDictionary<string, string> settings, CancellationToken ct);
    }
}
