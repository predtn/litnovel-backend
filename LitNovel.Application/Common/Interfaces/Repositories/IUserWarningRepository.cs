using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Staff;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface IUserWarningRepository
    {
        Task AddAsync(UserWarning warning, CancellationToken ct);
        Task<PagedResult<UserWarningResponseDto>> GetByUserIdAsync(int userId, int page, int size, CancellationToken ct);
        Task<int> CountByUserIdAsync(int userId, CancellationToken ct);
        Task<int> CountTotalActiveAsync(CancellationToken ct);
    }
}
