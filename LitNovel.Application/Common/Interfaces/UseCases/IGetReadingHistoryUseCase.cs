using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Reading;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetReadingHistoryUseCase
    {
        Task<PagedResult<ReadingProgressResponseDto>> ExecuteAsync(ReadingHistoryQueryDto query, CancellationToken ct);
    }
}
