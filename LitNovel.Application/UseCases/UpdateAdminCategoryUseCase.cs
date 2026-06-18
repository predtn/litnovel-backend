using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Category;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.UseCases
{
    public class UpdateAdminCategoryUseCase : IUpdateAdminCategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateCategoryRequestDto> _validator;

        public UpdateAdminCategoryUseCase(
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,
            IValidator<UpdateCategoryRequestDto> validator)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<CategoryResponseDto> ExecuteAsync(int id, UpdateCategoryRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var category = await _categoryRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Category not found");

            if (request.Name is not null)
            {
                var name = request.Name.Trim();
                if (await _categoryRepository.NameExistsAsync(name, id, ct))
                {
                    throw new ConflictException("Category name already exists");
                }

                category.Name = name;
            }

            if (request.Slug is not null)
            {
                var slug = request.Slug.Trim();
                if (await _categoryRepository.SlugExistsAsync(slug, id, ct))
                {
                    throw new ConflictException("Slug already exists");
                }

                category.Slug = slug;
            }

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
