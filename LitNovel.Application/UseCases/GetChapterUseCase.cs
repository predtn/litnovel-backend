using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Chapter;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class GetChapterUseCase : IGetChapterUseCase
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly IChapterReadRepository _chapterReadRepository;
        private readonly IReadingProgressRepository _readingProgressRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public GetChapterUseCase(
            IChapterRepository chapterRepository,
            IChapterReadRepository chapterReadRepository,
            IReadingProgressRepository readingProgressRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _chapterRepository = chapterRepository;
            _chapterReadRepository = chapterReadRepository;
            _readingProgressRepository = readingProgressRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ChapterDetailResponseDto> ExecuteAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid chapter id");
            }

            var chapter = await _chapterRepository.GetByIdWithDetailsAsync(id, ct);
            if (chapter == null)
            {
                throw new NotFoundException("Chapter not found");
            }

            var result = MapChapter(chapter);
            await MarkReadIfNeededAsync(chapter, ct);
            return result;
        }

        public async Task<ChapterDetailResponseDto> ExecuteBySlugAsync(string slug, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                throw new BadRequestException("Invalid chapter slug");
            }

            var chapter = await _chapterRepository.GetBySlugWithDetailsAsync(slug.Trim(), ct);
            if (chapter == null)
            {
                throw new NotFoundException("Chapter not found");
            }

            var result = MapChapter(chapter);
            await MarkReadIfNeededAsync(chapter, ct);
            return result;
        }

        private ChapterDetailResponseDto MapChapter(Chapter chapter)
        {
            if (!CanView(chapter))
            {
                throw new ForbiddenException("Chapter is not publicly available");
            }

            return new ChapterDetailResponseDto
            {
                Id = chapter.Id,
                ChapterNumber = chapter.ChapterNumber,
                Title = chapter.Title,
                Slug = chapter.Slug,
                Content = chapter.Content?.Content ?? string.Empty,
                Status = chapter.Status.ToString(),
                ReleaseDate = chapter.ReleaseDate,
                Volume = new ChapterVolumeResponseDto
                {
                    Id = chapter.Volume.Id,
                    VolumeNumber = chapter.Volume.VolumeNumber,
                    Title = chapter.Volume.Title
                },
                Novel = new ChapterNovelResponseDto
                {
                    Id = chapter.Volume.Novel.Id,
                    Title = chapter.Volume.Novel.Title,
                    Slug = chapter.Volume.Novel.Slug
                },
                CreatedAt = chapter.CreatedAt,
                UpdatedAt = chapter.UpdatedAt
            };
        }

        private bool CanView(Chapter chapter)
        {
            if (chapter.Status == ChapterStatus.Published && IsPublicStatus(chapter.Volume.Novel.Status))
            {
                return true;
            }

            return VolumePermissionHelper.CanManage(_currentUserService, chapter.Volume.Novel.AuthorId);
        }

        private static bool IsPublicStatus(NovelStatus status)
        {
            return status is NovelStatus.Ongoing or NovelStatus.Ended or NovelStatus.Hiatus or NovelStatus.Dropped;
        }

        private async Task MarkReadIfNeededAsync(Chapter chapter, CancellationToken ct)
        {
            if (!_currentUserService.IsAuthenticated
                || chapter.Status != ChapterStatus.Published
                || !IsPublicStatus(chapter.Volume.Novel.Status))
            {
                return;
            }

            var userId = _currentUserService.UserId;
            var novelId = chapter.Volume.NovelId;
            var now = DateTime.UtcNow;
            var existingRead = await _chapterReadRepository.GetByUserAndChapterAsync(userId, chapter.Id, ct);
            var isNewRead = existingRead is null;

            if (existingRead is null)
            {
                await _chapterReadRepository.AddAsync(new ChapterRead
                {
                    UserId = userId,
                    NovelId = novelId,
                    ChapterId = chapter.Id,
                    ReadAt = now
                }, ct);
            }
            else
            {
                existingRead.ReadAt = now;
            }

            var readCount = await _chapterReadRepository.CountReadPublishedChaptersByNovelAsync(userId, novelId, ct);
            if (isNewRead)
            {
                readCount++;
            }

            var totalPublishedChapters = await _chapterReadRepository.CountPublishedChaptersByNovelAsync(novelId, ct);
            var progressPercentage = totalPublishedChapters <= 0
                ? 0
                : Math.Min(100, (int)Math.Round(readCount * 100d / totalPublishedChapters, MidpointRounding.AwayFromZero));

            var progress = await _readingProgressRepository.GetByUserAndNovelAsync(userId, novelId, ct);
            if (progress is null)
            {
                await _readingProgressRepository.AddAsync(new ReadingProgress
                {
                    UserId = userId,
                    NovelId = novelId,
                    ChapterId = chapter.Id,
                    ProgressPercentage = (byte)progressPercentage,
                    LastReadAt = now
                }, ct);
            }
            else
            {
                progress.ChapterId = chapter.Id;
                progress.ProgressPercentage = (byte)progressPercentage;
                progress.LastReadAt = now;
            }

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
