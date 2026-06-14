using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface IStaffDashboardRepository
    {
        Task<StaffDashboardResponseDto> GetDashboardAsync(CancellationToken ct);
    }
}
