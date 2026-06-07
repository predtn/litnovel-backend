using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Category;

namespace LitNovel.Application.UseCases
{
    public class GetCategoriesUseCase : IGetCategoriesUseCase
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoriesUseCase(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Task<List<CategoryResponseDto>> ExecuteAsync(CancellationToken ct)
        {
            return _categoryRepository.GetAllAsync(ct);
        }
    }
}
