using LitNovel.Application.Common.Interfaces.Repositories;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LitNovelContext _context;

        public UnitOfWork(LitNovelContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct)
        {
            return _context.SaveChangesAsync(ct);
        }
    }
}
