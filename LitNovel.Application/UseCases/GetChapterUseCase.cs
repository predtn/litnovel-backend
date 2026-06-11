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
        private readonly ICurrentUserService _currentUserService;

        public GetChapterUseCase(
            IChapterRepository chapterRepository,
            ICurrentUserService currentUserService)
        {
            _chapterRepository = chapterRepository;
            _currentUserService = currentUserService;
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
    }
}
