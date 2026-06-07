using LitNovel.Application.DTOs.Tag;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetTagsUseCase
    {
        Task<List<TagResponseDto>> ExecuteAsync(CancellationToken ct);
    }
}
