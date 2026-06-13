using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Category;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.UseCases
{
    public class CreateAdminCategoryUseCase : ICreateAdminCategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateCategoryRequestDto> _validator;

        public CreateAdminCategoryUseCase(
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,
            IValidator<CreateCategoryRequestDto> validator)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<CategoryResponseDto> ExecuteAsync(CreateCategoryRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var name = request.Name.Trim();
            var slug = request.Slug.Trim();

            if (await _categoryRepository.NameExistsAsync(name, excludingId: null, ct))
            {
                throw new ConflictException("Category name already exists");
            }

            if (await _categoryRepository.SlugExistsAsync(slug, excludingId: null, ct))
            {
                throw new ConflictException("Slug already exists");
            }

            var category = new Category
            {
                Name = name,
                Slug = slug
            };

            await _categoryRepository.AddAsync(category, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return Map(category);
        }

        private static CategoryResponseDto Map(Category category)
        {
            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug
            };
        }
    }
}
