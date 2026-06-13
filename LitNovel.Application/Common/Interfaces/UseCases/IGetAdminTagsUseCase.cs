using LitNovel.Application.DTOs.Tag;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetAdminTagsUseCase
    {
        Task<IReadOnlyList<TagResponseDto>> ExecuteAsync(CancellationToken ct);
    }
}
