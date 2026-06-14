using LitNovel.Application.DTOs.Category;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<CategoryResponseDto>> GetAllAsync(CancellationToken ct);
        Task<Category?> GetByIdAsync(int id, CancellationToken ct);
        Task<bool> ExistsAsync(int id, CancellationToken ct);
        Task<bool> NameExistsAsync(string name, int? excludingId, CancellationToken ct);
        Task<bool> SlugExistsAsync(string slug, int? excludingId, CancellationToken ct);
        Task AddAsync(Category category, CancellationToken ct);
        void Delete(Category category);
    }
}
