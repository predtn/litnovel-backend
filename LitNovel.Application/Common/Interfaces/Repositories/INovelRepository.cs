using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Novel;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface INovelRepository
    {
        Task<PagedResult<NovelListItemResponseDto>> GetListAsync(NovelListQueryDto query, CancellationToken ct);
    }
}
