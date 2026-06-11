using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Domain.Entities;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class NovelReportRepository : INovelReportRepository
    {
        private readonly LitNovelContext _context;

        public NovelReportRepository(LitNovelContext context)
        {
            _context = context;
        }

        public Task AddAsync(NovelReport report, CancellationToken ct)
        {
            return _context.NovelReports.AddAsync(report, ct).AsTask();
        }
    }
}
