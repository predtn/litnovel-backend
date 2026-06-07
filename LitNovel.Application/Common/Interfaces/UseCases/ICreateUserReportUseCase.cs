using LitNovel.Application.DTOs.Report;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface ICreateUserReportUseCase
    {
        Task<CreateUserReportResponseDto> ExecuteAsync(CreateUserReportRequestDto request, CancellationToken ct);
    }
}
