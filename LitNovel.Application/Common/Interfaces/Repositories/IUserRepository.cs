using LitNovel.Domain.Entities;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.User;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id, CancellationToken ct);
        Task<User?> GetByIdWithProfileAsync(int id, CancellationToken ct);
        Task<User?> GetByIdentifierAsync(string identifier, CancellationToken ct);
        Task<PagedResult<UserSearchResponseDto>> SearchAsync(UserSearchQueryDto query, CancellationToken ct);
        Task<bool> EmailExistsAsync(string email, CancellationToken ct);
        Task<bool> UsernameExistsAsync(string username, CancellationToken ct);
        Task AddAsync(User user, CancellationToken ct);
    }
}
