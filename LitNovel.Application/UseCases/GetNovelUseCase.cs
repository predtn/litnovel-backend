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
        private readonly ICurrentUserService _currentUserService;

        public GetNovelUseCase(INovelRepository novelRepository, ICurrentUserService currentUserService)
        {
            _novelRepository = novelRepository;
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

            if (!CanView(novel))
            {
                throw new ForbiddenException("Novel is not publicly available");
            }

            var canManage = CanManage(novel);
            var visibleVolumes = novel.Volumes
                .OrderBy(v => v.VolumeNumber)
                .Select(v => new NovelDetailVolumeResponseDto
                {
                    Id = v.Id,
                    VolumeNumber = v.VolumeNumber,
                    Title = v.Title,
                    Chapters = v.Chapters
                        .Where(c => canManage || c.Status == ChapterStatus.Published)
                        .OrderBy(c => c.ChapterNumber)
                        .Select(c => new NovelDetailChapterResponseDto
                        {
                            Id = c.Id,
                            ChapterNumber = c.ChapterNumber,
                            Title = c.Title,
                            Status = c.Status.ToString(),
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
                TotalChapters = canManage ? novel.TotalChapters : visibleVolumes.Sum(v => v.Chapters.Count),
                TotalVolumes = canManage ? novel.TotalVolumes : visibleVolumes.Count,
                RatingAverage = novel.NovelRatings.Any() ? novel.NovelRatings.Average(r => r.Rating) : 0,
                RatingCount = novel.NovelRatings.Count,
                Volumes = visibleVolumes,
                CreatedAt = novel.CreatedAt,
                UpdatedAt = novel.UpdatedAt
            };
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
            return novel.AuthorId == _currentUserService.UserId
                || string.Equals(_currentUserService.Role, UserRole.Staff.ToString(), StringComparison.OrdinalIgnoreCase)
                || string.Equals(_currentUserService.Role, UserRole.Admin.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsPublicStatus(NovelStatus status)
        {
            return status is NovelStatus.Ongoing or NovelStatus.Ended or NovelStatus.Hiatus or NovelStatus.Dropped;
        }
    }
}
