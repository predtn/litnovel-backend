using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.UseCases
{
    public class GetNovelForReviewUseCase : IGetNovelForReviewUseCase
    {
        private readonly INovelRepository _novelRepository;

        public GetNovelForReviewUseCase(INovelRepository novelRepository)
        {
            _novelRepository = novelRepository;
        }

        public async Task<NovelReviewDetailResponseDto> ExecuteAsync(int novelId, CancellationToken ct)
        {
            var novel = await _novelRepository.GetByIdForModerationAsync(novelId, ct)
                ?? throw new NotFoundException("Novel not found.");

            return new NovelReviewDetailResponseDto
            {
                Id              = novel.Id,
                Title           = novel.Title,
                Slug            = novel.Slug,
                Description     = novel.Description,
                CoverImage      = novel.CoverImage,
                Status          = novel.Status.ToString(),
                AuthorId        = novel.Author.Id,
                AuthorUsername  = novel.Author.Username,
                AuthorAvatar    = novel.Author.Avatar,
                CategoryName    = novel.Category?.Name,
                Tags            = novel.NovelTags.Select(nt => nt.Tag.Name).ToList(),
                TotalChapters   = novel.TotalChapters,
                TotalVolumes    = novel.TotalVolumes,
                ViewCount       = novel.ViewCount,
                CreatedAt       = novel.CreatedAt,
                UpdatedAt       = novel.UpdatedAt,
                Volumes         = novel.Volumes
                    .OrderBy(v => v.VolumeNumber)
                    .Select(v => new NovelReviewVolumeDto
                    {
                        Id           = v.Id,
                        VolumeNumber = v.VolumeNumber,
                        Title        = v.Title,
                        Chapters     = v.Chapters
                            .OrderBy(c => c.ChapterNumber)
                            .Select(c => new NovelReviewChapterDto
                            {
                                Id            = c.Id,
                                ChapterNumber = c.ChapterNumber,
                                Title         = c.Title,
                                Status        = c.Status.ToString()
                            }).ToList()
                    }).ToList()
            };
        }
    }
}
