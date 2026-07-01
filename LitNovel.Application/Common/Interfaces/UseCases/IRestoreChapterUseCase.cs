using LitNovel.Application.DTOs.Chapter;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IRestoreChapterUseCase
    {
        Task<UpdateChapterResponseDto> ExecuteAsync(int id, CancellationToken ct);
    }
}
