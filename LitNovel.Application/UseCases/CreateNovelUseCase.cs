using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Novel;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class CreateNovelUseCase : ICreateNovelUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateNovelRequestDto> _validator;

        public CreateNovelUseCase(
            INovelRepository novelRepository,
            ICategoryRepository categoryRepository,
            ITagRepository tagRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IValidator<CreateNovelRequestDto> validator)
        {
            _novelRepository = novelRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<CreateNovelResponseDto> ExecuteAsync(CreateNovelRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);
            await EnsureReferencesExistAsync(request.CategoryId, request.TagIds, ct);

            var slug = await CreateUniqueSlugAsync(request.Title, null, ct);
            var novel = new Novel
            {
                Title = request.Title.Trim(),
                Slug = slug,
                Description = request.Description,
                CoverImage = request.CoverImage,
                CategoryId = request.CategoryId,
                AuthorId = _currentUserService.UserId,
                Status = NovelStatus.Draft
            };

            foreach (var tagId in request.TagIds ?? new List<int>())
            {
                novel.NovelTags.Add(new NovelTag { Novel = novel, TagId = tagId });
            }

            await _novelRepository.AddAsync(novel, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return new CreateNovelResponseDto
            {
                Id = novel.Id,
                Title = novel.Title,
                Slug = novel.Slug,
                Status = novel.Status.ToString(),
                CreatedAt = novel.CreatedAt
            };
        }

        private async Task EnsureReferencesExistAsync(int? categoryId, List<int>? tagIds, CancellationToken ct)
        {
            if (categoryId.HasValue && !await _categoryRepository.ExistsAsync(categoryId.Value, ct))
            {
                throw new BadRequestException("Invalid category ID");
            }

            if (tagIds is { Count: > 0 })
            {
                var existingIds = await _tagRepository.GetExistingIdsAsync(tagIds, ct);
                if (existingIds.Count != tagIds.Count)
                {
                    throw new BadRequestException("One or more tag IDs are invalid");
                }
            }
        }

        private async Task<string> CreateUniqueSlugAsync(string title, int? excludeNovelId, CancellationToken ct)
        {
            var baseSlug = NovelSlugGenerator.Generate(title);
            var slug = baseSlug;
            var suffix = 2;

            while (await _novelRepository.SlugExistsAsync(slug, excludeNovelId, ct))
            {
                slug = $"{baseSlug}-{suffix}";
                suffix++;
            }

            return slug;
        }
    }
}
