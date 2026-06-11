using LitNovel.Application.DTOs.Chapter;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface ISubmitChapterUseCase
    {
        Task<SubmitChapterResponseDto> ExecuteAsync(int id, CancellationToken ct);
    }
}
