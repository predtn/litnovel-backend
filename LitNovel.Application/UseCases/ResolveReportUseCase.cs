using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Notification;
using LitNovel.Application.DTOs.Staff;
using LitNovel.Domain.Common;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class ResolveReportUseCase : IResolveReportUseCase
    {
        private readonly INovelReportRepository      _novelReportRepository;
        private readonly IUserReportRepository       _userReportRepository;
        private readonly IModerationLogRepository    _moderationLogRepository;
        private readonly INotificationRepository     _notificationRepository;
        private readonly ICommentChapterRepository   _commentChapterRepository;
        private readonly INotificationPushService    _notificationPush;
        private readonly ICurrentUserService         _currentUserService;
        private readonly IUnitOfWork                 _unitOfWork;
        private readonly IValidator<ResolveReportRequestDto> _validator;

        public ResolveReportUseCase(
            INovelReportRepository novelReportRepository,
            IUserReportRepository userReportRepository,
            IModerationLogRepository moderationLogRepository,
            INotificationRepository notificationRepository,
            ICommentChapterRepository commentChapterRepository,
            INotificationPushService notificationPush,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IValidator<ResolveReportRequestDto> validator)
        {
            _novelReportRepository    = novelReportRepository;
            _userReportRepository     = userReportRepository;
            _moderationLogRepository  = moderationLogRepository;
            _notificationRepository   = notificationRepository;
            _commentChapterRepository = commentChapterRepository;
            _notificationPush         = notificationPush;
            _currentUserService       = currentUserService;
            _unitOfWork               = unitOfWork;
            _validator                = validator;
        }

        public async Task ExecuteAsync(int reportId, string kind, ResolveReportRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var staffId = _currentUserService.UserId;

            var action = request.Action.Trim();
            ReportStatus newStatus = action.ToLowerInvariant() switch
            {
                "resolve" => ReportStatus.Resolved,
                "reject"  => ReportStatus.Rejected,
                _         => throw new BadRequestException($"Invalid action '{request.Action}'. Valid: Resolve, Reject.")
            };
            string logAction = newStatus == ReportStatus.Resolved ? "ResolveReport" : "RejectReport";

            var normalizedKind = kind?.Trim().ToLowerInvariant();
            string targetTitle;
            int targetId = reportId;
            int reporterId;

            string? takeDownDetail = null;

            if (normalizedKind == "novel")
            {
                var report = await _novelReportRepository.GetByIdWithDetailsAsync(reportId, ct)
                    ?? throw new NotFoundException("Report not found.");

                ApplyResolution(report, newStatus, staffId, request);
                targetTitle = report.TargetNovel?.Title ?? $"Novel Report #{reportId}";
                reporterId  = report.ReporterId;

                // Gỡ nội dung: chuyển Chapter báo cáo về Draft
                if (request.TakeDownContent && newStatus == ReportStatus.Resolved && report.TargetChapter is not null)
                {
                    report.TargetChapter.Status = ChapterStatus.Draft;
                    takeDownDetail = $"Chapter \"{ report.TargetChapter.Title}\" đã được chuyển về Draft.";
                }
            }
            else if (normalizedKind == "user")
            {
                var report = await _userReportRepository.GetByIdWithDetailsAsync(reportId, ct)
                    ?? throw new NotFoundException("Report not found.");

                ApplyResolution(report, newStatus, staffId, request);
                targetTitle = report.TargetUser?.Username ?? $"User Report #{reportId}";
                reporterId  = report.ReporterId;

                // Gỡ nội dung: xóa Comment vi phạm
                if (request.TakeDownContent && newStatus == ReportStatus.Resolved && report.TargetComment is not null)
                {
                    _commentChapterRepository.Delete(report.TargetComment);
                    takeDownDetail = $"Comment #{report.TargetCommentId} đã được xóa.";
                }
            }
            else
            {
                throw new BadRequestException("Kind must be 'Novel' or 'User'.");
            }

            var notes = string.IsNullOrWhiteSpace(takeDownDetail)
                ? request.ResolutionNotes
                : $"{request.ResolutionNotes} | TakeDown: {takeDownDetail}";

            var log = new ModerationLog
            {
                StaffId     = staffId,
                Action      = logAction,
                TargetType  = "Report",
                TargetId    = targetId,
                TargetTitle = targetTitle,
                Notes       = notes,
                PerformedAt = DateTime.UtcNow
            };

            // Trigger ReportUpdate notification to the original reporter
            string notifMessage = newStatus == ReportStatus.Resolved
                ? $"Báo cáo của bạn về \"{targetTitle}\" đã được xử lý và giải quyết."
                : $"Báo cáo của bạn về \"{targetTitle}\" đã được xem xét nhưng không được chấp nhận.";

            var notification = new Notification
            {
                UserId           = reporterId,
                NotificationType = NotificationType.ReportUpdate,
                EntityType       = "Report",
                EntityId         = reportId,
                Message          = notifMessage,
                IsRead           = false
            };

            await _notificationRepository.AddAsync(notification, ct);
            await _moderationLogRepository.AddAsync(log, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            var pushDto = new NotificationResponseDto
            {
                Id               = notification.Id,
                NotificationType = notification.NotificationType.ToString(),
                EntityType       = notification.EntityType,
                EntityId         = notification.EntityId,
                Message          = notification.Message,
                IsRead           = false,
                CreatedAt        = notification.CreatedAt
            };
            await _notificationPush.PushAsync(reporterId, pushDto, ct);
        }

        private static void ApplyResolution(BaseReport report, ReportStatus status, int staffId, ResolveReportRequestDto request)
        {
            report.Status          = status;
            report.ProcessedById   = staffId;
            report.ActionTaken     = request.ActionTaken;
            report.ResolutionNotes = request.ResolutionNotes;
        }
    }
}
