using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Reading;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class SaveReadingProgressUseCase : ISaveReadingProgressUseCase
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly IReadingProgressRepository _readingProgressRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IValidator<SaveReadingProgressRequestDto> _validator;

        public SaveReadingProgressUseCase(
            IChapterRepository chapterRepository,
            IReadingProgressRepository readingProgressRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IValidator<SaveReadingProgressRequestDto> validator)
        {
            _chapterRepository = chapterRepository;
            _readingProgressRepository = readingProgressRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _validator = validator;
        }

        public async Task<SaveReadingProgressResponseDto> ExecuteAsync(int chapterId, SaveReadingProgressRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var chapter = await _chapterRepository.GetByIdWithDetailsAsync(chapterId, ct)
                ?? throw new NotFoundException("Chapter not found");

            if (chapter.Status != ChapterStatus.Published)
            {
                throw new ForbiddenException("Chapter is not publicly available");
            }

            var novelId = chapter.Volume.NovelId;
            var progress = await _readingProgressRepository.GetByUserAndNovelAsync(_currentUserService.UserId, novelId, ct);
            var now = DateTime.UtcNow;

            if (progress is null)
            {
                progress = new ReadingProgress
                {
                    UserId = _currentUserService.UserId,
                    NovelId = novelId,
                    ChapterId = chapterId,
                    ProgressPercentage = (byte)request.ProgressPercentage,
                    LastReadAt = now
                };
                await _readingProgressRepository.AddAsync(progress, ct);
            }
            else
            {
                progress.ChapterId = chapterId;
                progress.ProgressPercentage = (byte)request.ProgressPercentage;
                progress.LastReadAt = now;
            }

            await _unitOfWork.SaveChangesAsync(ct);

            return new SaveReadingProgressResponseDto
            {
                NovelId = novelId,
                ChapterId = chapterId,
                ProgressPercentage = request.ProgressPercentage,
                LastReadAt = now
            };
        }
    }
}
