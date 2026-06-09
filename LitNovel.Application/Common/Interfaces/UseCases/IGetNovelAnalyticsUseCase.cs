using LitNovel.Application.DTOs.Novel;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetNovelAnalyticsUseCase
    {
        Task<NovelAnalyticsResponseDto> ExecuteAsync(int id, CancellationToken ct);
    }
}
