using LitNovel.Application.DTOs.Novel;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetNovelUseCase
    {
        Task<NovelDetailResponseDto> ExecuteAsync(int id, CancellationToken ct);
        Task<NovelDetailResponseDto> ExecuteBySlugAsync(string slug, CancellationToken ct);
    }
}
