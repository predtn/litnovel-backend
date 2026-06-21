using FluentValidation;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.UseCases
{
    public class GetAdminAuditLogsUseCase : IGetAdminAuditLogsUseCase
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IValidator<AdminAuditLogQueryDto> _validator;

        public GetAdminAuditLogsUseCase(
            IAuditLogRepository auditLogRepository,
            IValidator<AdminAuditLogQueryDto> validator)
        {
            _auditLogRepository = auditLogRepository;
            _validator = validator;
        }

        public async Task<PagedResult<AdminAuditLogResponseDto>> ExecuteAsync(AdminAuditLogQueryDto query, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(query, ct);
            return await _auditLogRepository.GetAdminAuditLogsAsync(query, ct);
        }

        public IQueryable<AdminAuditLogResponseDto> ExecuteQuery()
        {
            return _auditLogRepository.QueryAdminAuditLogs();
        }
    }
}
