using LitNovel.Application.DTOs.Chapter;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface ICreateChapterUseCase
    {
        Task<ChapterResponseDto> ExecuteAsync(int volumeId, CreateChapterRequestDto request, CancellationToken ct);
    }
}
