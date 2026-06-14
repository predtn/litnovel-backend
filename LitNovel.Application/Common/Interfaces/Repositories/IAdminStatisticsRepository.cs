using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface IAdminStatisticsRepository
    {
        Task<AdminStatisticsResponseDto> GetStatisticsAsync(CancellationToken ct);
        Task<AdminStatisticsChartResponseDto> GetStatisticsChartAsync(AdminStatisticsChartQueryDto query, CancellationToken ct);
    }
}
