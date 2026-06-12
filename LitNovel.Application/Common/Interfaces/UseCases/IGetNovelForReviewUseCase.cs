using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetNovelForReviewUseCase
    {
        Task<NovelReviewDetailResponseDto> ExecuteAsync(int novelId, CancellationToken ct);
    }
}
