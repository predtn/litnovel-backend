using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetStaffDashboardUseCase
    {
        Task<StaffDashboardResponseDto> ExecuteAsync(CancellationToken ct);
    }
}
