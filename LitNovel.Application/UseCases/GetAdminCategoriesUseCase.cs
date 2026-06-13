using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Category;

namespace LitNovel.Application.UseCases
{
    public class GetAdminCategoriesUseCase : IGetAdminCategoriesUseCase
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetAdminCategoriesUseCase(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IReadOnlyList<CategoryResponseDto>> ExecuteAsync(CancellationToken ct)
        {
            return await _categoryRepository.GetAllAsync(ct);
        }
    }
}
