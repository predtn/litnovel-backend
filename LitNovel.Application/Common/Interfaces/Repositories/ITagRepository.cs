using LitNovel.Application.DTOs.Tag;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface ITagRepository
    {
        Task<List<TagResponseDto>> GetAllAsync(CancellationToken ct);
        Task<Tag?> GetByIdAsync(int id, CancellationToken ct);
        Task<List<int>> GetExistingIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct);
        Task<bool> NameExistsAsync(string name, int? excludingId, CancellationToken ct);
        Task<bool> SlugExistsAsync(string slug, int? excludingId, CancellationToken ct);
        Task AddAsync(Tag tag, CancellationToken ct);
        void Delete(Tag tag);
    }
}
