using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id, CancellationToken ct);
        Task<User?> GetByIdWithProfileAsync(int id, CancellationToken ct);
        Task<User?> GetByIdentifierAsync(string identifier, CancellationToken ct);
        Task<bool> EmailExistsAsync(string email, CancellationToken ct);
        Task<bool> UsernameExistsAsync(string username, CancellationToken ct);
        Task AddAsync(User user, CancellationToken ct);
    }
}
