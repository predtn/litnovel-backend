using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IResolveReportUseCase
    {
        /// <param name="kind">"Novel" | "User"</param>
        Task ExecuteAsync(int reportId, string kind, ResolveReportRequestDto request, CancellationToken ct);
    }
}
