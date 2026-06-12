using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Staff;
using LitNovel.Domain.Common;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class ResolveReportUseCase : IResolveReportUseCase
    {
        private readonly INovelReportRepository _novelReportRepository;
        private readonly IUserReportRepository  _userReportRepository;
        private readonly ICurrentUserService    _currentUserService;
        private readonly IUnitOfWork            _unitOfWork;

        public ResolveReportUseCase(
            INovelReportRepository novelReportRepository,
            IUserReportRepository userReportRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _novelReportRepository = novelReportRepository;
            _userReportRepository  = userReportRepository;
            _currentUserService    = currentUserService;
            _unitOfWork            = unitOfWork;
        }

        public async Task ExecuteAsync(int reportId, string kind, ResolveReportRequestDto request, CancellationToken ct)
        {
            var staffId = _currentUserService.UserId;
            if (staffId <= 0) throw new UnauthorizedException("Not authenticated.");

            var action = request.Action?.Trim().ToLowerInvariant()
                ?? throw new BadRequestException("Action is required.");

            ReportStatus newStatus = action switch
            {
                "resolve" => ReportStatus.Resolved,
                "reject"  => ReportStatus.Rejected,
                _         => throw new BadRequestException($"Invalid action '{request.Action}'. Valid: Resolve, Reject.")
            };

            var normalizedKind = kind?.Trim().ToLowerInvariant();

            if (normalizedKind == "novel")
            {
                var report = await _novelReportRepository.GetByIdWithDetailsAsync(reportId, ct)
                    ?? throw new NotFoundException("Report not found.");

                ApplyResolution(report, newStatus, staffId, request);
            }
            else if (normalizedKind == "user")
            {
                var report = await _userReportRepository.GetByIdWithDetailsAsync(reportId, ct)
                    ?? throw new NotFoundException("Report not found.");

                ApplyResolution(report, newStatus, staffId, request);
            }
            else
            {
                throw new BadRequestException("Kind must be 'Novel' or 'User'.");
            }

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
