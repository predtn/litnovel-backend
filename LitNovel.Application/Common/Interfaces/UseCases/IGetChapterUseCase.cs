using LitNovel.Application.DTOs.Chapter;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetChapterUseCase
    {
        Task<ChapterDetailResponseDto> ExecuteAsync(int id, CancellationToken ct);
    }
}
