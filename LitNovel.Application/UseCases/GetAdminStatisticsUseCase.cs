using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.UseCases
{
    public class GetAdminStatisticsUseCase : IGetAdminStatisticsUseCase
    {
        private readonly IAdminStatisticsRepository _adminStatisticsRepository;

        public GetAdminStatisticsUseCase(IAdminStatisticsRepository adminStatisticsRepository)
        {
            _adminStatisticsRepository = adminStatisticsRepository;
        }

        public Task<AdminStatisticsResponseDto> ExecuteAsync(CancellationToken ct)
        {
            return _adminStatisticsRepository.GetStatisticsAsync(ct);
        }
    }
}
