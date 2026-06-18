using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Staff;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class WarnUserUseCase : IWarnUserUseCase
    {
        private readonly IUserRepository          _userRepository;
        private readonly IUserWarningRepository   _userWarningRepository;
        private readonly INotificationRepository  _notificationRepository;
        private readonly IModerationLogRepository _moderationLogRepository;
        private readonly ICurrentUserService      _currentUserService;
        private readonly IUnitOfWork              _unitOfWork;
        private readonly IValidator<WarnUserRequestDto> _validator;

        public WarnUserUseCase(
            IUserRepository userRepository,
            IUserWarningRepository userWarningRepository,
            INotificationRepository notificationRepository,
            IModerationLogRepository moderationLogRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IValidator<WarnUserRequestDto> validator)
        {
            _userRepository          = userRepository;
            _userWarningRepository   = userWarningRepository;
            _notificationRepository  = notificationRepository;
            _moderationLogRepository = moderationLogRepository;
            _currentUserService      = currentUserService;
            _unitOfWork              = unitOfWork;
            _validator               = validator;
        }

        public async Task ExecuteAsync(int userId, WarnUserRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var staffId = _currentUserService.UserId;

            if (userId <= 0)
                throw new BadRequestException("Invalid user ID.");

            var user = await _userRepository.GetByIdAsync(userId, ct)
                ?? throw new NotFoundException("User not found.");

            var severity = Enum.TryParse<WarningSeverity>(request.Severity, true, out var parsedSeverity)
                ? parsedSeverity
                : throw new BadRequestException("Severity must be 'Minor' or 'Major'.");

            var warning = new UserWarning
            {
                UserId     = user.Id,
                IssuedById = staffId,
                Reason     = request.Reason,
                Severity   = severity
            };

            var notification = new Notification
            {
                UserId           = user.Id,
                NotificationType = NotificationType.SystemAlert,
                EntityType       = "Warning",
                EntityId         = null,
                Message          = $"⚠️ [{severity}] Cảnh báo từ Ban kiểm duyệt: {request.Reason}",
                IsRead           = false
            };

            var log = new ModerationLog
            {
                StaffId     = staffId,
                Action      = "WarnUser",
                TargetType  = "User",
                TargetId    = user.Id,
                TargetTitle = user.Username,
                Notes       = $"[{severity}] {request.Reason}",
                PerformedAt = DateTime.UtcNow
            };

            await _userWarningRepository.AddAsync(warning, ct);
            await _notificationRepository.AddAsync(notification, ct);
            await _moderationLogRepository.AddAsync(log, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
