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
    public class UpdateNovelUseCase : IUpdateNovelUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateNovelRequestDto> _validator;

        public UpdateNovelUseCase(
            INovelRepository novelRepository,
            ICategoryRepository categoryRepository,
            ITagRepository tagRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IValidator<UpdateNovelRequestDto> validator)
        {
            _novelRepository = novelRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<UpdateNovelResponseDto> ExecuteAsync(int id, UpdateNovelRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);
            if (id <= 0)
            {
                throw new BadRequestException("Invalid novel id");
            }

            var novel = await _novelRepository.GetByIdWithTagsForUpdateAsync(id, ct);
            if (novel == null)
            {
                throw new NotFoundException("Novel not found");
            }

            if (!CanManage(novel.AuthorId))
            {
                throw new ForbiddenException("You do not have permission to edit this novel");
            }

            if (novel.Status == NovelStatus.Pending)
            {
                throw new BadRequestException("Withdraw the novel submission before editing");
            }

            if (novel.Status == NovelStatus.Locked)
            {
                throw new BadRequestException("Locked novel cannot be edited");
            }

            await EnsureReferencesExistAsync(request.CategoryId, request.TagIds, ct);

            var title = request.Title.Trim();
            if (await _novelRepository.TitleExistsForAuthorAsync(novel.AuthorId, title, novel.Id, ct))
            {
                throw new ConflictException("Novel title already exists");
            }

            var slug = await CreateUniqueSlugAsync(title, novel.Id, ct);
            var shouldResubmitForReview = novel.Status != NovelStatus.Draft;
            if (shouldResubmitForReview && IsPublicStatus(novel.Status))
            {
                novel.PreviousPublicStatus = novel.Status;
            }

            novel.Title = title;
            novel.Slug = slug;
            novel.Description = request.Description;
            novel.CoverImage = request.CoverImage;
            novel.CategoryId = request.CategoryId;
            ReplaceTags(novel, request.TagIds ?? new List<int>());

            if (shouldResubmitForReview)
            {
                novel.Status = NovelStatus.Pending;
            }

            await _unitOfWork.SaveChangesAsync(ct);

            return new UpdateNovelResponseDto
            {
                Id = novel.Id,
                Title = title,
                Slug = slug,
                Status = novel.Status.ToString(),
                UpdatedAt = novel.UpdatedAt
            };
        }

        private bool CanManage(int authorId)
        {
            return authorId == _currentUserService.UserId
                || string.Equals(_currentUserService.Role, UserRole.Staff.ToString(), StringComparison.OrdinalIgnoreCase)
                || string.Equals(_currentUserService.Role, UserRole.Admin.ToString(), StringComparison.OrdinalIgnoreCase);
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

        private async Task<string> CreateUniqueSlugAsync(string title, int excludeNovelId, CancellationToken ct)
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

        private static void ReplaceTags(Novel novel, List<int> tagIds)
        {
            var existing = novel.NovelTags.ToList();
            foreach (var novelTag in existing.Where(nt => !tagIds.Contains(nt.TagId)))
            {
                novel.NovelTags.Remove(novelTag);
            }

            var existingIds = novel.NovelTags.Select(nt => nt.TagId).ToHashSet();
            foreach (var tagId in tagIds.Where(tagId => !existingIds.Contains(tagId)))
            {
                novel.NovelTags.Add(new NovelTag { NovelId = novel.Id, TagId = tagId });
            }
        }

        private static bool IsPublicStatus(NovelStatus status)
        {
            return status is NovelStatus.Ongoing or NovelStatus.Ended or NovelStatus.Hiatus or NovelStatus.Dropped;
        }

    }
}
