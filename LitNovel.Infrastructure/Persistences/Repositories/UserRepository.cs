using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Application.DTOs.User;
using LitNovel.Domain.Enums;
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

        public async Task<PagedResult<UserSearchResponseDto>> SearchAsync(UserSearchQueryDto query, CancellationToken ct)
        {
            var page = query.Page <= 0 ? 1 : query.Page;
            var size = query.Size <= 0 ? 10 : query.Size;

            var users = _context.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                var keyword = query.Keyword.Trim();
                users = users.Where(u => u.Username.Contains(keyword));
            }

            var total = await users.CountAsync(ct);
            var items = await users
                .OrderBy(u => u.Username)
                .Skip((page - 1) * size)
                .Take(size)
                .Select(u => new UserSearchResponseDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Avatar = u.Avatar,
                    NovelsPublished = u.Novels.Count(n => n.Status == NovelStatus.Ongoing
                        || n.Status == NovelStatus.Ended
                        || n.Status == NovelStatus.Hiatus
                        || n.Status == NovelStatus.Dropped)
                })
                .ToListAsync(ct);

            return new PagedResult<UserSearchResponseDto>
            {
                Items = items,
                Page = page,
                Size = size,
                TotalElements = total,
                TotalPages = (int)Math.Ceiling(total / (double)size)
            };
        }

        public IQueryable<AdminUserListItemResponseDto> QueryAdminUsers()
        {
            return _context.Users
                .AsNoTracking()
                .Select(u => new AdminUserListItemResponseDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Avatar = u.Avatar,
                    Role = u.Role.ToString(),
                    Status = u.Status.ToString(),
                    NovelsCount = u.Novels.Count,
                    JoinedAt = u.CreatedAt
                });
        }

        public Task<int> CountByRoleAsync(UserRole role, CancellationToken ct)
        {
            return _context.Users.AsNoTracking().CountAsync(u => u.Role == role, ct);
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

        public void Delete(User user)
        {
            _context.Users.Remove(user);
        }
    }
}
