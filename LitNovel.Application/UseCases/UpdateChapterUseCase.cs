using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Chapter;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class UpdateChapterUseCase : IUpdateChapterUseCase
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateChapterRequestDto> _validator;

        public UpdateChapterUseCase(
            IChapterRepository chapterRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IValidator<UpdateChapterRequestDto> validator)
        {
            _chapterRepository = chapterRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<UpdateChapterResponseDto> ExecuteAsync(int id, UpdateChapterRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);
            if (id <= 0)
            {
                throw new BadRequestException("Invalid chapter id");
            }

            var chapter = await _chapterRepository.GetByIdForUpdateAsync(id, ct);
            if (chapter == null)
            {
                throw new NotFoundException("Chapter not found");
            }

            if (!VolumePermissionHelper.CanManage(_currentUserService, chapter.Volume.Novel.AuthorId))
            {
                throw new ForbiddenException("You do not have permission to edit this novel");
            }

            if (chapter.Status == ChapterStatus.Pending)
            {
                throw new BadRequestException("Withdraw the chapter submission before editing");
            }

            if (chapter.Status == ChapterStatus.Locked)
            {
                throw new BadRequestException("Locked chapter cannot be edited");
            }

            if (await _chapterRepository.ChapterNumberExistsAsync(chapter.VolumeId, request.ChapterNumber, id, ct))
            {
                throw new ConflictException("Chapter number already exists in this volume");
            }

            var title = request.Title.Trim();
            var slug = NovelSlugGenerator.Generate($"{chapter.VolumeId}-{request.ChapterNumber}-{request.Title}");
            var shouldResubmitForReview = chapter.Status != ChapterStatus.Draft;
            chapter.ChapterNumber = request.ChapterNumber;
            chapter.Title = title;
            chapter.Slug = slug;
            chapter.ReleaseDate = request.ReleaseDate;
            chapter.Content ??= new ChapterContent { ChapterId = chapter.Id };
            chapter.Content.Content = request.Content.Trim();
            chapter.Content.Version++;

            if (shouldResubmitForReview)
            {
                chapter.Status = ChapterStatus.Pending;
            }

            await _unitOfWork.SaveChangesAsync(ct);

            return new UpdateChapterResponseDto
            {
                Id = chapter.Id,
                Title = title,
                Status = chapter.Status.ToString(),
                UpdatedAt = chapter.UpdatedAt
            };
        }
    }
}
