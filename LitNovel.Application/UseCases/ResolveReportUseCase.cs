using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Staff;
using LitNovel.Domain.Common;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class ResolveReportUseCase : IResolveReportUseCase
    {
        private readonly INovelReportRepository   _novelReportRepository;
        private readonly IUserReportRepository    _userReportRepository;
        private readonly IModerationLogRepository _moderationLogRepository;
        private readonly ICurrentUserService      _currentUserService;
        private readonly IUnitOfWork              _unitOfWork;
        private readonly IValidator<ResolveReportRequestDto> _validator;

        public ResolveReportUseCase(
            INovelReportRepository novelReportRepository,
            IUserReportRepository userReportRepository,
            IModerationLogRepository moderationLogRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IValidator<ResolveReportRequestDto> validator)
        {
            _novelReportRepository   = novelReportRepository;
            _userReportRepository    = userReportRepository;
            _moderationLogRepository = moderationLogRepository;
            _currentUserService      = currentUserService;
            _unitOfWork              = unitOfWork;
            _validator               = validator;
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

            if (normalizedKind == "novel")
            {
                var report = await _novelReportRepository.GetByIdWithDetailsAsync(reportId, ct)
                    ?? throw new NotFoundException("Report not found.");

                ApplyResolution(report, newStatus, staffId, request);
                targetTitle = report.TargetNovel?.Title ?? $"Novel Report #{reportId}";
            }
            else if (normalizedKind == "user")
            {
                var report = await _userReportRepository.GetByIdWithDetailsAsync(reportId, ct)
                    ?? throw new NotFoundException("Report not found.");

                ApplyResolution(report, newStatus, staffId, request);
                targetTitle = report.TargetUser?.Username ?? $"User Report #{reportId}";
            }
            else
            {
                throw new BadRequestException("Kind must be 'Novel' or 'User'.");
            }

            var log = new ModerationLog
            {
                StaffId     = staffId,
                Action      = logAction,
                TargetType  = "Report",
                TargetId    = targetId,
                TargetTitle = targetTitle,
                Notes       = request.ResolutionNotes,
                PerformedAt = DateTime.UtcNow
            };

            await _moderationLogRepository.AddAsync(log, ct);
            await _unitOfWork.SaveChangesAsync(ct);
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
