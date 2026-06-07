using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Domain.Entities;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class UserReportRepository : IUserReportRepository
    {
        private readonly LitNovelContext _context;

        public UserReportRepository(LitNovelContext context)
        {
            _context = context;
        }

        public Task AddAsync(UserReport report, CancellationToken ct)
        {
            return _context.UserReports.AddAsync(report, ct).AsTask();
        }
    }
}
