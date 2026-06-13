using FluentValidation;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.UseCases
{
    public class GetAdminReportsUseCase : IGetAdminReportsUseCase
    {
        private readonly IReportRepository _reportRepository;
        private readonly IValidator<AdminReportsQueryDto> _validator;

        public GetAdminReportsUseCase(IReportRepository reportRepository, IValidator<AdminReportsQueryDto> validator)
        {
            _reportRepository = reportRepository;
            _validator = validator;
        }

        public async Task<PagedResult<AdminReportResponseDto>> ExecuteAsync(AdminReportsQueryDto query, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(query, ct);
            return await _reportRepository.GetAdminReportsAsync(query, ct);
        }
    }
}
