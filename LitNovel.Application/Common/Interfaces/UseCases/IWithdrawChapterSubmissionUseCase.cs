using LitNovel.Application.DTOs.Chapter;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IWithdrawChapterSubmissionUseCase
    {
        Task<SubmitChapterResponseDto> ExecuteAsync(int id, CancellationToken ct);
    }
}
