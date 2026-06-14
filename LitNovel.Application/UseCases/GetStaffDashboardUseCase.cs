using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.UseCases
{
    public class GetStaffDashboardUseCase : IGetStaffDashboardUseCase
    {
        private readonly IStaffDashboardRepository _staffDashboardRepository;

        public GetStaffDashboardUseCase(IStaffDashboardRepository staffDashboardRepository)
        {
            _staffDashboardRepository = staffDashboardRepository;
        }

        public Task<StaffDashboardResponseDto> ExecuteAsync(CancellationToken ct)
        {
            return _staffDashboardRepository.GetDashboardAsync(ct);
        }
    }
}
