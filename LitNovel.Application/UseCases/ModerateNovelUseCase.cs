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
    public class ModerateNovelUseCase : IModerateNovelUseCase
    {
        private readonly INovelRepository        _novelRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IModerationLogRepository _moderationLogRepository;
        private readonly ICurrentUserService      _currentUserService;
        private readonly IUnitOfWork              _unitOfWork;
        private readonly IValidator<ModerateNovelRequestDto> _validator;

        public ModerateNovelUseCase(
            INovelRepository novelRepository,
            INotificationRepository notificationRepository,
            IModerationLogRepository moderationLogRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IValidator<ModerateNovelRequestDto> validator)
        {
            _novelRepository         = novelRepository;
            _notificationRepository  = notificationRepository;
            _moderationLogRepository = moderationLogRepository;
            _currentUserService      = currentUserService;
            _unitOfWork              = unitOfWork;
            _validator               = validator;
        }

        public async Task ExecuteAsync(int novelId, ModerateNovelRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var staffId = _currentUserService.UserId;

            var novel = await _novelRepository.GetByIdForUpdateAsync(novelId, ct)
                ?? throw new NotFoundException("Novel not found.");

            if (novel.Status != NovelStatus.Pending)
                throw new BadRequestException("Novel is not in Pending status.");

            var action = request.Action.Trim();
            NovelStatus newStatus;
            string notificationMessage;
            string logAction;

            switch (action.ToLowerInvariant())
            {
                case "approve":
                    newStatus = NovelStatus.Ongoing;
                    notificationMessage = $"Tiểu thuyết \"{novel.Title}\" của bạn đã được phê duyệt và xuất bản.";
                    logAction = "ApproveNovel";
                    break;
                case "reject":
                    newStatus = NovelStatus.Draft;
                    notificationMessage = $"Tiểu thuyết \"{novel.Title}\" bị từ chối." +
                                         (string.IsNullOrWhiteSpace(request.Reason) ? "" : $" Lý do: {request.Reason}");
                    logAction = "RejectNovel";
                    break;
                case "lock":
                    newStatus = NovelStatus.Locked;
                    notificationMessage = $"Tiểu thuyết \"{novel.Title}\" đã bị khóa." +
                                         (string.IsNullOrWhiteSpace(request.Reason) ? "" : $" Lý do: {request.Reason}");
                    logAction = "LockNovel";
                    break;
                default:
                    throw new BadRequestException($"Invalid action '{request.Action}'. Valid: Approve, Reject, Lock.");
            }

            novel.Status = newStatus;

            var notification = new Notification
            {
                UserId           = novel.AuthorId,
                NotificationType = NotificationType.SystemAlert,
                EntityType       = "Novel",
                EntityId         = novel.Id,
                Message          = notificationMessage,
                IsRead           = false
            };

            var log = new ModerationLog
            {
                StaffId     = staffId,
                Action      = logAction,
                TargetType  = "Novel",
                TargetId    = novel.Id,
                TargetTitle = novel.Title,
                Notes       = request.Reason,
                PerformedAt = DateTime.UtcNow
            };

            await _notificationRepository.AddAsync(notification, ct);
            await _moderationLogRepository.AddAsync(log, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
