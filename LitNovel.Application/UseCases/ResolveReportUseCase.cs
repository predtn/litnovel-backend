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
        private readonly IUserRepository             _userRepository;
        private readonly IUserWarningRepository      _userWarningRepository;
        private readonly IRefreshTokenRepository     _refreshTokenRepository;
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
            IUserRepository userRepository,
            IUserWarningRepository userWarningRepository,
            IRefreshTokenRepository refreshTokenRepository,
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
            _userRepository           = userRepository;
            _userWarningRepository    = userWarningRepository;
            _refreshTokenRepository   = refreshTokenRepository;
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
            int? targetUserIdToNotify = null;
            string? targetNotificationMessage = null;

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
                    
                    if (report.TargetNovel?.AuthorId != null)
                    {
                        targetUserIdToNotify = report.TargetNovel.AuthorId;
                        targetNotificationMessage = $"Chương \"{report.TargetChapter.Title}\" của truyện \"{report.TargetNovel.Title}\" đã bị chuyển về bản nháp do vi phạm nội quy.";
                    }
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
                    targetUserIdToNotify = report.TargetComment.UserId;
                    targetNotificationMessage = "Một bình luận của bạn đã bị xóa do vi phạm nội quy.";

                    _commentChapterRepository.Delete(report.TargetComment);
                    takeDownDetail = $"Comment #{report.TargetCommentId} đã được xóa.";
                }
            }
            else
            {
                throw new BadRequestException("Kind must be 'Novel' or 'User'.");
            }

            if (newStatus == ReportStatus.Rejected && (request.TakeDownContent || request.WarnUser || request.BanUser))
            {
                throw new BadRequestException("Không thể áp dụng hình phạt (Gỡ bài, Cảnh cáo, Khóa tài khoản) khi Bác bỏ báo cáo.");
            }

            if ((request.WarnUser || request.BanUser) && !request.TargetUserId.HasValue)
            {
                throw new BadRequestException("Thiếu TargetUserId để áp dụng hình phạt.");
            }

            string? penaltyDetail = null;
            var notificationsToPush = new List<Notification>();

            if (newStatus == ReportStatus.Resolved && request.TargetUserId.HasValue && (request.WarnUser || request.BanUser))
            {
                var targetUser = await _userRepository.GetByIdAsync(request.TargetUserId.Value, ct);
                if (targetUser != null && targetUser.Role != UserRole.Admin && targetUser.Role != UserRole.Staff)
                {
                    if (request.WarnUser)
                    {
                        var warning = new UserWarning
                        {
                            UserId = targetUser.Id,
                            IssuedById = staffId,
                            Reason = request.ResolutionNotes ?? "Vi phạm nội quy.",
                            Severity = WarningSeverity.Major
                        };
                        await _userWarningRepository.AddAsync(warning, ct);
                        penaltyDetail = "Đã cảnh cáo người dùng.";
                        
                        var warnNotif = new Notification
                        {
                            UserId = targetUser.Id,
                            NotificationType = NotificationType.SystemAlert,
                            EntityType = "User",
                            EntityId = targetUser.Id,
                            Message = $"Bạn đã nhận 1 cảnh báo vi phạm. Lý do: {warning.Reason}",
                            IsRead = false
                        };
                        await _notificationRepository.AddAsync(warnNotif, ct);
                        notificationsToPush.Add(warnNotif);
                    }

                    if (request.BanUser)
                    {
                        targetUser.Status = UserStatus.Banned;
                        await _refreshTokenRepository.RevokeAllForUserAsync(targetUser.Id, ct);
                        penaltyDetail = (penaltyDetail == null) ? "Đã khóa tài khoản." : penaltyDetail + " Đã khóa tài khoản.";
                    }
                }
            }

            var notesList = new List<string> { request.ResolutionNotes ?? "" };
            if (!string.IsNullOrWhiteSpace(takeDownDetail)) notesList.Add($"TakeDown: {takeDownDetail}");
            if (!string.IsNullOrWhiteSpace(penaltyDetail)) notesList.Add($"Penalty: {penaltyDetail}");
            var notes = string.Join(" | ", notesList.Where(s => !string.IsNullOrWhiteSpace(s)));

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
            await _moderationLogRepository.AddAsync(log, ct);

            // Trigger ReportUpdate notification to the original reporter
            string notifMessage = newStatus == ReportStatus.Resolved
                ? $"Báo cáo của bạn về \"{targetTitle}\" đã được xử lý và giải quyết."
                : $"Báo cáo của bạn về \"{targetTitle}\" đã được xem xét nhưng không được chấp nhận.";

            var reporterNotification = new Notification
            {
                UserId           = reporterId,
                NotificationType = NotificationType.ReportUpdate,
                EntityType       = "Report",
                EntityId         = reportId,
                Message          = notifMessage,
                IsRead           = false
            };
            await _notificationRepository.AddAsync(reporterNotification, ct);
            notificationsToPush.Add(reporterNotification);

            if (targetUserIdToNotify.HasValue && !string.IsNullOrEmpty(targetNotificationMessage))
            {
                var targetNotification = new Notification
                {
                    UserId           = targetUserIdToNotify.Value,
                    NotificationType = NotificationType.SystemAlert,
                    EntityType       = normalizedKind == "novel" ? "Chapter" : "Comment",
                    EntityId         = normalizedKind == "novel" ? targetId : reportId,
                    Message          = targetNotificationMessage,
                    IsRead           = false
                };
                await _notificationRepository.AddAsync(targetNotification, ct);
                notificationsToPush.Add(targetNotification);
            }

            await _unitOfWork.SaveChangesAsync(ct);

            foreach (var notif in notificationsToPush)
            {
                var pushDto = new NotificationResponseDto
                {
                    Id               = notif.Id,
                    NotificationType = notif.NotificationType.ToString(),
                    EntityType       = notif.EntityType,
                    EntityId         = notif.EntityId,
                    Message          = notif.Message,
                    IsRead           = false,
                    CreatedAt        = notif.CreatedAt
                };
                await _notificationPush.PushAsync(notif.UserId, pushDto, ct);
            }
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
