using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface IUserReportRepository
    {
        Task AddAsync(UserReport report, CancellationToken ct);
    }
}
