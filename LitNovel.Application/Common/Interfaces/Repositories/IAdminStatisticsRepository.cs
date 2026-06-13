using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface IAdminStatisticsRepository
    {
        Task<AdminStatisticsResponseDto> GetStatisticsAsync(CancellationToken ct);
    }
}
