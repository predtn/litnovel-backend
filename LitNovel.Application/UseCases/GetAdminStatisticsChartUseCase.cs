using FluentValidation;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.UseCases
{
    public class GetAdminStatisticsChartUseCase : IGetAdminStatisticsChartUseCase
    {
        private readonly IAdminStatisticsRepository _adminStatisticsRepository;
        private readonly IValidator<AdminStatisticsChartQueryDto> _validator;

        public GetAdminStatisticsChartUseCase(
            IAdminStatisticsRepository adminStatisticsRepository,
            IValidator<AdminStatisticsChartQueryDto> validator)
        {
            _adminStatisticsRepository = adminStatisticsRepository;
            _validator = validator;
        }

        public async Task<AdminStatisticsChartResponseDto> ExecuteAsync(AdminStatisticsChartQueryDto query, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(query, ct);
            return await _adminStatisticsRepository.GetStatisticsChartAsync(query, ct);
        }
    }
}
