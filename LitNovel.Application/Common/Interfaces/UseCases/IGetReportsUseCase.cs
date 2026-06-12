using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetReportsUseCase
    {
        /// <param name="kind">null = all | "Novel" | "User"</param>
        Task<PagedResult<ReportListItemResponseDto>> ExecuteAsync(string? kind, string? status, int page, int size, CancellationToken ct);
    }
}
