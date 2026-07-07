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

            var result = await MapChapterAsync(chapter, ct);
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

            var result = await MapChapterAsync(chapter, ct);
            await MarkReadIfNeededAsync(chapter, ct);
            return result;
        }

        private async Task<ChapterDetailResponseDto> MapChapterAsync(Chapter chapter, CancellationToken ct)
        {
            if (!CanView(chapter))
            {
                throw new ForbiddenException("Chapter is not publicly available");
            }

            var novelId = chapter.Volume.NovelId;
            var prevChapter = await _chapterRepository.GetPreviousPublicChapterAsync(novelId, chapter.ChapterNumber, ct);
            var nextChapter = await _chapterRepository.GetNextPublicChapterAsync(novelId, chapter.ChapterNumber, ct);

            return new ChapterDetailResponseDto
            {
                Id = chapter.Id,
                ChapterNumber = chapter.ChapterNumber,
                Title = chapter.Title,
                Slug = chapter.Slug,
                Content = chapter.Content?.Content ?? string.Empty,
                Status = chapter.Status.ToString(),
                ReleaseDate = chapter.ReleaseDate,
                DeletionRequestedAt = chapter.DeletionRequestedAt,
                ScheduledHardDeleteAt = chapter.ScheduledHardDeleteAt,
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
                PrevChapter = MapNavChapter(prevChapter),
                NextChapter = MapNavChapter(nextChapter),
                CreatedAt = chapter.CreatedAt,
                UpdatedAt = chapter.UpdatedAt
            };
        }

        private static ChapterNavResponseDto? MapNavChapter(Chapter? chapter)
        {
            if (chapter == null)
            {
                return null;
            }

            return new ChapterNavResponseDto
            {
                Id = chapter.Id,
                Slug = chapter.Slug,
                VolumeId = chapter.VolumeId,
                ChapterNumber = chapter.ChapterNumber,
                Title = chapter.Title,
                Status = chapter.Status.ToString(),
                CreatedAt = chapter.CreatedAt,
                UpdatedAt = chapter.UpdatedAt,
                DeletionRequestedAt = chapter.DeletionRequestedAt,
                ScheduledHardDeleteAt = chapter.ScheduledHardDeleteAt
            };
        }

        private bool CanView(Chapter chapter)
        {
            if (IsPublicChapterStatus(chapter.Status) && IsPublicStatus(chapter.Volume.Novel.Status))
            {
                return true;
            }

            return VolumePermissionHelper.CanManage(_currentUserService, chapter.Volume.Novel.AuthorId);
        }

        private static bool IsPublicStatus(NovelStatus status)
        {
            return status is NovelStatus.Ongoing or NovelStatus.Ended or NovelStatus.Hiatus or NovelStatus.Dropped or NovelStatus.PendingDeletion;
        }

        private static bool IsPublicChapterStatus(ChapterStatus status)
        {
            return status is ChapterStatus.Published or ChapterStatus.PendingDeletion;
        }

        private async Task MarkReadIfNeededAsync(Chapter chapter, CancellationToken ct)
        {
            if (!_currentUserService.IsAuthenticated
                || !IsPublicChapterStatus(chapter.Status)
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
