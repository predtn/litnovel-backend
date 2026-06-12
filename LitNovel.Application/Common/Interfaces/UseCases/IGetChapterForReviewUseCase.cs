using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetChapterForReviewUseCase
    {
        Task<ChapterReviewDetailResponseDto> ExecuteAsync(int chapterId, CancellationToken ct);
    }
}
