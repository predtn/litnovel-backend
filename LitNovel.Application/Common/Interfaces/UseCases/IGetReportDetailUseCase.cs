using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetReportDetailUseCase
    {
        /// <param name="kind">"Novel" | "User"</param>
        Task<ReportDetailResponseDto> ExecuteAsync(int reportId, string kind, CancellationToken ct);
    }
}
