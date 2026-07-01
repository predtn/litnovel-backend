using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Novel;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class GetNovelUseCase : IGetNovelUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly IChapterReadRepository _chapterReadRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetNovelUseCase(
            INovelRepository novelRepository,
            IChapterReadRepository chapterReadRepository,
            ICurrentUserService currentUserService)
        {
            _novelRepository = novelRepository;
            _chapterReadRepository = chapterReadRepository;
            _currentUserService = currentUserService;
        }

        public async Task<NovelDetailResponseDto> ExecuteAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid novel id");
            }

            var novel = await _novelRepository.GetByIdWithDetailsAsync(id, ct);
            if (novel == null)
            {
                throw new NotFoundException("Novel not found");
            }

            return await MapNovelAsync(novel, ct);
        }

        public async Task<NovelDetailResponseDto> ExecuteBySlugAsync(string slug, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                throw new BadRequestException("Invalid novel slug");
            }

            var novel = await _novelRepository.GetBySlugWithDetailsAsync(slug.Trim(), ct);
            if (novel == null)
            {
                throw new NotFoundException("Novel not found");
            }

            return await MapNovelAsync(novel, ct);
        }

        private async Task<NovelDetailResponseDto> MapNovelAsync(Novel novel, CancellationToken ct)
        {
            if (!CanView(novel))
            {
                throw new ForbiddenException("Novel is not publicly available");
            }

            var canManage = CanManage(novel);
            var readChapterIds = _currentUserService.IsAuthenticated
                ? await _chapterReadRepository.GetReadChapterIdsByNovelAsync(_currentUserService.UserId, novel.Id, ct)
                : new HashSet<int>();
            var currentUserRating = _currentUserService.IsAuthenticated
                ? novel.NovelRatings.FirstOrDefault(r => r.UserId == _currentUserService.UserId)
                : null;
            var visibleVolumes = novel.Volumes
                .OrderBy(v => v.VolumeNumber)
                .Select(v => new NovelDetailVolumeResponseDto
                {
                    Id = v.Id,
                    VolumeNumber = v.VolumeNumber,
                    Title = v.Title,
                    Chapters = v.Chapters
                        .Where(c => canManage || IsPublicChapterStatus(c.Status))
                        .OrderBy(c => c.ChapterNumber)
                        .Select(c => new NovelDetailChapterResponseDto
                        {
                            Id = c.Id,
                            Slug = c.Slug,
                            ChapterNumber = c.ChapterNumber,
                            Title = c.Title,
                            Status = c.Status.ToString(),
                            IsRead = readChapterIds.Contains(c.Id),
                            DeletionRequestedAt = c.DeletionRequestedAt,
                            ScheduledHardDeleteAt = c.ScheduledHardDeleteAt,
                            CreatedAt = c.CreatedAt
                        })
                        .ToList()
                })
                .Where(v => canManage || v.Chapters.Count > 0)
                .ToList();

            return new NovelDetailResponseDto
            {
                Id = novel.Id,
                Title = novel.Title,
                Slug = novel.Slug,
                CoverImage = novel.CoverImage,
                Description = novel.Description,
                Author = new NovelAuthorResponseDto
                {
                    Id = novel.Author.Id,
                    Username = novel.Author.Username,
                    Avatar = novel.Author.Avatar
                },
                Category = novel.Category == null ? null : new NovelCategoryResponseDto
                {
                    Id = novel.Category.Id,
                    Name = novel.Category.Name
                },
                Tags = novel.NovelTags
                    .Select(nt => new NovelTagResponseDto { Id = nt.Tag.Id, Name = nt.Tag.Name })
                    .ToList(),
                Status = novel.Status.ToString(),
                ViewCount = novel.ViewCount,
                LikeCount = novel.LikeCount,
                IsFavorited = _currentUserService.IsAuthenticated
                    ? novel.Favorites.Any(f => f.UserId == _currentUserService.UserId)
                    : null,
                IsLiked = _currentUserService.IsAuthenticated
                    ? novel.NovelLikes.Any(l => l.UserId == _currentUserService.UserId)
                    : null,
                UserReviewId = currentUserRating?.Id,
                UserRating = currentUserRating?.Rating,
                UserReview = currentUserRating?.Review,
                TotalChapters = canManage ? novel.TotalChapters : visibleVolumes.Sum(v => v.Chapters.Count),
                TotalVolumes = canManage ? novel.TotalVolumes : visibleVolumes.Count,
                ReadChapterCount = visibleVolumes
                    .SelectMany(v => v.Chapters)
                    .Count(c => c.IsRead && string.Equals(c.Status, ChapterStatus.Published.ToString(), StringComparison.OrdinalIgnoreCase)),
                ReadingProgressPercentage = CalculateReadingProgress(visibleVolumes),
                RatingAverage = novel.NovelRatings.Any() ? novel.NovelRatings.Average(r => r.Rating) : 0,
                RatingCount = novel.NovelRatings.Count,
                DeletionRequestedAt = novel.DeletionRequestedAt,
                ScheduledHardDeleteAt = novel.ScheduledHardDeleteAt,
                Volumes = visibleVolumes,
                CreatedAt = novel.CreatedAt,
                UpdatedAt = novel.UpdatedAt
            };
        }

        private static int CalculateReadingProgress(IEnumerable<NovelDetailVolumeResponseDto> volumes)
        {
            var chapters = volumes
                .SelectMany(v => v.Chapters)
                .Where(c => string.Equals(c.Status, ChapterStatus.Published.ToString(), StringComparison.OrdinalIgnoreCase)
                    || string.Equals(c.Status, ChapterStatus.PendingDeletion.ToString(), StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (chapters.Count == 0)
            {
                return 0;
            }

            var readCount = chapters.Count(c => c.IsRead);
            return Math.Min(100, (int)Math.Round(readCount * 100d / chapters.Count, MidpointRounding.AwayFromZero));
        }

        private bool CanView(Novel novel)
        {
            if (IsPublicStatus(novel.Status))
            {
                return true;
            }

            return _currentUserService.IsAuthenticated
                && CanManage(novel);
        }

        private bool CanManage(Novel novel)
        {
            if (!_currentUserService.IsAuthenticated)
            {
                return false;
            }

            return novel.AuthorId == _currentUserService.UserId
                || string.Equals(_currentUserService.Role, UserRole.Staff.ToString(), StringComparison.OrdinalIgnoreCase)
                || string.Equals(_currentUserService.Role, UserRole.Admin.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsPublicStatus(NovelStatus status)
        {
            return status is NovelStatus.Ongoing or NovelStatus.Ended or NovelStatus.Hiatus or NovelStatus.Dropped or NovelStatus.PendingDeletion;
        }

        private static bool IsPublicChapterStatus(ChapterStatus status)
        {
            return status is ChapterStatus.Published or ChapterStatus.PendingDeletion;
        }
    }
}
