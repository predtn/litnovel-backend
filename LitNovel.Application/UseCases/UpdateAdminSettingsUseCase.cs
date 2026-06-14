using FluentValidation;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.UseCases
{
    public class UpdateAdminSettingsUseCase : IUpdateAdminSettingsUseCase
    {
        private readonly ISystemSettingRepository _systemSettingRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateAdminSettingsRequestDto> _validator;

        public UpdateAdminSettingsUseCase(
            ISystemSettingRepository systemSettingRepository,
            IAuditLogRepository auditLogRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IValidator<UpdateAdminSettingsRequestDto> validator)
        {
            _systemSettingRepository = systemSettingRepository;
            _auditLogRepository = auditLogRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<AdminSettingsResponseDto> ExecuteAsync(UpdateAdminSettingsRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var values = AdminSettingsMapper.ToSettingValues(request);
            await _systemSettingRepository.UpsertRangeAsync(values, ct);
            await _auditLogRepository.AddAsync(new AuditLog
            {
                ActorId = _currentUserService.UserId,
                Action = "UpdateSystemSettings",
                EntityType = "SystemSettings",
                EntityId = 0,
                IpAddress = _currentUserService.IpAddress
            }, ct);

            await _unitOfWork.SaveChangesAsync(ct);

            var settings = await _systemSettingRepository.GetAllAsync(ct);
            return AdminSettingsMapper.ToResponse(settings);
        }
    }
}
