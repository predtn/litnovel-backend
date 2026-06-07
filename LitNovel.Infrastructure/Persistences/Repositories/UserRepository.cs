using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LitNovelContext _context;

        public UserRepository(LitNovelContext context)
        {
            _context = context;
        }

        public Task<User?> GetByIdAsync(int id, CancellationToken ct)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        public Task<User?> GetByIdWithProfileAsync(int id, CancellationToken ct)
        {
            return _context.Users
                .AsNoTracking()
                .Include(u => u.Reputation)
                .Include(u => u.UserBadges)
                    .ThenInclude(ub => ub.Badge)
                .Include(u => u.Novels)
                    .ThenInclude(n => n.Volumes)
                    .ThenInclude(v => v.Chapters)
                .Include(u => u.Favorites)
                .Include(u => u.CommentChapters)
                .AsSplitQuery()
                .FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        public Task<User?> GetByIdentifierAsync(string identifier, CancellationToken ct)
        {
            var normalized = identifier.Trim();
            return _context.Users.FirstOrDefaultAsync(u => u.Email == normalized || u.Username == normalized, ct);
        }

        public Task<bool> EmailExistsAsync(string email, CancellationToken ct)
        {
            var normalized = email.Trim();
            return _context.Users.AsNoTracking().AnyAsync(u => u.Email == normalized, ct);
        }

        public Task<bool> UsernameExistsAsync(string username, CancellationToken ct)
        {
            var normalized = username.Trim();
            return _context.Users.AsNoTracking().AnyAsync(u => u.Username == normalized, ct);
        }

        public Task AddAsync(User user, CancellationToken ct)
        {
            return _context.Users.AddAsync(user, ct).AsTask();
        }
    }
}
