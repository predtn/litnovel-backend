using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetAdminStatisticsChartUseCase
    {
        Task<AdminStatisticsChartResponseDto> ExecuteAsync(AdminStatisticsChartQueryDto query, CancellationToken ct);
    }
}
