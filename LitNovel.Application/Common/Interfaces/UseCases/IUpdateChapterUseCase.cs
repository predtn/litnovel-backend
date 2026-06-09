using LitNovel.Application.DTOs.Chapter;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IUpdateChapterUseCase
    {
        Task<UpdateChapterResponseDto> ExecuteAsync(int id, UpdateChapterRequestDto request, CancellationToken ct);
    }
}
