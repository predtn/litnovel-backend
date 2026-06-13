using LitNovel.Application.DTOs.Category;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetAdminCategoriesUseCase
    {
        Task<IReadOnlyList<CategoryResponseDto>> ExecuteAsync(CancellationToken ct);
    }
}
