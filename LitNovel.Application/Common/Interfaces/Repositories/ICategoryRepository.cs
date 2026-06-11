using LitNovel.Application.DTOs.Category;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<CategoryResponseDto>> GetAllAsync(CancellationToken ct);
        Task<bool> ExistsAsync(int id, CancellationToken ct);
    }
}
