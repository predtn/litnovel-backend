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
    public class ModerateChapterUseCase : IModerateChapterUseCase
    {
        private readonly IChapterRepository      _chapterRepository;
        private readonly INotificationRepository  _notificationRepository;
        private readonly IModerationLogRepository _moderationLogRepository;
        private readonly ICurrentUserService      _currentUserService;
        private readonly IUnitOfWork              _unitOfWork;
        private readonly IValidator<ModerateChapterRequestDto> _validator;

        public ModerateChapterUseCase(
            IChapterRepository chapterRepository,
            INotificationRepository notificationRepository,
            IModerationLogRepository moderationLogRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IValidator<ModerateChapterRequestDto> validator)
        {
            _chapterRepository       = chapterRepository;
            _notificationRepository  = notificationRepository;
            _moderationLogRepository = moderationLogRepository;
            _currentUserService      = currentUserService;
            _unitOfWork              = unitOfWork;
            _validator               = validator;
        }

        public async Task ExecuteAsync(int chapterId, ModerateChapterRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var staffId = _currentUserService.UserId;

            var chapter = await _chapterRepository.GetByIdForUpdateAsync(chapterId, ct)
                ?? throw new NotFoundException("Chapter not found.");

            if (chapter.Status != ChapterStatus.Pending)
                throw new BadRequestException("Chapter is not in Pending status.");

            var action = request.Action.Trim();
            ChapterStatus newStatus;
            string notificationMessage;
            string logAction;
            int authorId = chapter.Volume.Novel.AuthorId;

            switch (action.ToLowerInvariant())
            {
                case "approve":
                    newStatus = ChapterStatus.Published;
                    notificationMessage = $"Chương \"{chapter.Title}\" của bạn đã được phê duyệt và xuất bản.";
                    logAction = "ApproveChapter";
                    break;
                case "reject":
                    newStatus = ChapterStatus.Draft;
                    notificationMessage = $"Chương \"{chapter.Title}\" bị từ chối." +
                                         (string.IsNullOrWhiteSpace(request.Reason) ? "" : $" Lý do: {request.Reason}");
                    logAction = "RejectChapter";
                    break;
                case "lock":
                    newStatus = ChapterStatus.Locked;
                    notificationMessage = $"Chương \"{chapter.Title}\" đã bị khóa." +
                                         (string.IsNullOrWhiteSpace(request.Reason) ? "" : $" Lý do: {request.Reason}");
                    logAction = "LockChapter";
                    break;
                default:
                    throw new BadRequestException($"Invalid action '{request.Action}'. Valid: Approve, Reject, Lock.");
            }

            chapter.Status = newStatus;

            var notification = new Notification
            {
                UserId           = authorId,
                NotificationType = NotificationType.SystemAlert,
                EntityType       = "Chapter",
                EntityId         = chapter.Id,
                Message          = notificationMessage,
                IsRead           = false
            };

            var log = new ModerationLog
            {
                StaffId     = staffId,
                Action      = logAction,
                TargetType  = "Chapter",
                TargetId    = chapter.Id,
                TargetTitle = chapter.Title,
                Notes       = request.Reason,
                PerformedAt = DateTime.UtcNow
            };

            await _notificationRepository.AddAsync(notification, ct);
            await _moderationLogRepository.AddAsync(log, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
