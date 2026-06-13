using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;

namespace LitNovel.Application.UseCases
{
    public class DeleteAdminCategoryUseCase : IDeleteAdminCategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAdminCategoryUseCase(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(int id, CancellationToken ct)
        {
            var category = await _categoryRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Category not found");

            _categoryRepository.Delete(category);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
