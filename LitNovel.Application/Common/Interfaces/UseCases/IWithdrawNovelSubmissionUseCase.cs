using LitNovel.Application.DTOs.Novel;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IWithdrawNovelSubmissionUseCase
    {
        Task<SubmitNovelResponseDto> ExecuteAsync(int id, CancellationToken ct);
    }
}
