using LitNovel.Application.DTOs.Report;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface ICreateNovelReportUseCase
    {
        Task<CreateNovelReportResponseDto> ExecuteAsync(CreateNovelReportRequestDto request, CancellationToken ct);
    }
}
