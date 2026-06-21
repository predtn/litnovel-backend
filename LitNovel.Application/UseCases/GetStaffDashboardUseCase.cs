using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.UseCases
{
    public class GetStaffDashboardUseCase : IGetStaffDashboardUseCase
    {
        private readonly INovelRepository         _novelRepository;
        private readonly IChapterRepository       _chapterRepository;
        private readonly INovelReportRepository   _novelReportRepository;
        private readonly IUserReportRepository    _userReportRepository;
        private readonly IUserWarningRepository   _userWarningRepository;
        private readonly IModerationLogRepository _moderationLogRepository;

        public GetStaffDashboardUseCase(
            INovelRepository novelRepository,
            IChapterRepository chapterRepository,
            INovelReportRepository novelReportRepository,
            IUserReportRepository userReportRepository,
            IUserWarningRepository userWarningRepository,
            IModerationLogRepository moderationLogRepository)
        {
            _novelRepository         = novelRepository;
            _chapterRepository       = chapterRepository;
            _novelReportRepository   = novelReportRepository;
            _userReportRepository    = userReportRepository;
            _userWarningRepository   = userWarningRepository;
            _moderationLogRepository = moderationLogRepository;
        }

        public async Task<StaffDashboardResponseDto> ExecuteAsync(CancellationToken ct)
        {
            // ⚠️ EF Core DbContext is NOT thread-safe — must await sequentially
            var pendingNovels   = await _novelRepository.CountPendingAsync(ct);
            var pendingChapters = await _chapterRepository.CountPendingAsync(ct);
            var novelReports    = await _novelReportRepository.CountPendingAsync(ct);
            var userReports     = await _userReportRepository.CountPendingAsync(ct);
            var activeWarnings  = await _userWarningRepository.CountTotalActiveAsync(ct);
            var recentActivity  = await _moderationLogRepository.GetRecentAsync(10, ct);

            return new StaffDashboardResponseDto
            {
                PendingNovels   = pendingNovels,
                PendingChapters = pendingChapters,
                OpenReports     = novelReports + userReports,
                ActiveWarnings  = activeWarnings,
                RecentActivity  = recentActivity.Select(r => new ModerationActivityDto
                {
                    Action      = r.Action,
                    StaffUsername = r.StaffUsername,
                    Target      = r.TargetTitle ?? $"{r.TargetType} #{r.TargetId}",
                    PerformedAt = r.PerformedAt
                }).ToList()
            };
        }
    }
}
