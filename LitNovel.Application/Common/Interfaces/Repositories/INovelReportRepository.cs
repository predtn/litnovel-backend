using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface INovelReportRepository
    {
        Task AddAsync(NovelReport report, CancellationToken ct);
    }
}
