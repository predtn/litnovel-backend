using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.UseCases
{
    public class GetChapterForReviewUseCase : IGetChapterForReviewUseCase
    {
        private readonly IChapterRepository _chapterRepository;

        public GetChapterForReviewUseCase(IChapterRepository chapterRepository)
        {
            _chapterRepository = chapterRepository;
        }

        public async Task<ChapterReviewDetailResponseDto> ExecuteAsync(int chapterId, CancellationToken ct)
        {
            var chapter = await _chapterRepository.GetByIdForModerationAsync(chapterId, ct)
                ?? throw new NotFoundException("Chapter not found.");

            return new ChapterReviewDetailResponseDto
            {
                Id            = chapter.Id,
                ChapterNumber = chapter.ChapterNumber,
                Title         = chapter.Title,
                Slug          = chapter.Slug,
                Status        = chapter.Status.ToString(),
                Content       = chapter.Content?.Content ?? string.Empty,
                VolumeId      = chapter.Volume.Id,
                VolumeTitle   = chapter.Volume.Title,
                NovelId       = chapter.Volume.Novel.Id,
                NovelTitle    = chapter.Volume.Novel.Title,
                NovelSlug     = chapter.Volume.Novel.Slug,
                AuthorId      = chapter.Volume.Novel.AuthorId,
                AuthorUsername = chapter.Volume.Novel.Author.Username,
                CreatedAt     = chapter.CreatedAt,
                UpdatedAt     = chapter.UpdatedAt
            };
        }
    }
}
