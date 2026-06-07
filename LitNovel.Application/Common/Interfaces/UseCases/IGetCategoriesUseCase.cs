using LitNovel.Application.DTOs.Category;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetCategoriesUseCase
    {
        Task<List<CategoryResponseDto>> ExecuteAsync(CancellationToken ct);
    }
}
