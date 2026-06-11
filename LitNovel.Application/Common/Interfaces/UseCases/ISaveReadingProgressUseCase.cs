using LitNovel.Application.DTOs.Reading;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface ISaveReadingProgressUseCase
    {
        Task<SaveReadingProgressResponseDto> ExecuteAsync(int chapterId, SaveReadingProgressRequestDto request, CancellationToken ct);
    }
}
