using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Chapter;
using LitNovel.Domain.Entities;

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

            if (await _chapterRepository.ChapterNumberExistsAsync(chapter.VolumeId, request.ChapterNumber, id, ct))
            {
                throw new ConflictException("Chapter number already exists in this volume");
            }

            chapter.ChapterNumber = request.ChapterNumber;
            chapter.Title = request.Title.Trim();
            chapter.Slug = NovelSlugGenerator.Generate($"{chapter.VolumeId}-{request.ChapterNumber}-{request.Title}");
            chapter.ReleaseDate = request.ReleaseDate;
            chapter.Content ??= new ChapterContent { ChapterId = chapter.Id };
            chapter.Content.Content = request.Content.Trim();
            chapter.Content.Version++;

            await _unitOfWork.SaveChangesAsync(ct);

            return new UpdateChapterResponseDto
            {
                Id = chapter.Id,
                Title = chapter.Title,
                Status = chapter.Status.ToString(),
                UpdatedAt = chapter.UpdatedAt
            };
        }
    }
}
