using LitNovel.Application.DTOs.Tag;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface ITagRepository
    {
        Task<List<TagResponseDto>> GetAllAsync(CancellationToken ct);
        Task<List<int>> GetExistingIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct);
    }
}
