using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.DTOs.Staff;
using LitNovel.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class StaffDashboardRepository : IStaffDashboardRepository
    {
        private readonly LitNovelContext _context;

        public StaffDashboardRepository(LitNovelContext context)
        {
            _context = context;
        }

        public async Task<StaffDashboardResponseDto> GetDashboardAsync(CancellationToken ct)
        {
            var pendingNovels = await _context.Novels
                .AsNoTracking()
                .CountAsync(n => n.Status == NovelStatus.Pending, ct);

            var pendingChapters = await _context.Chapters
                .AsNoTracking()
                .CountAsync(c => c.Status == ChapterStatus.Pending, ct);

            var openNovelReports = await _context.NovelReports
                .AsNoTracking()
                .CountAsync(r => r.Status == ReportStatus.Pending, ct);

            var openUserReports = await _context.UserReports
                .AsNoTracking()
                .CountAsync(r => r.Status == ReportStatus.Pending, ct);

            var recentActivities = await _context.AuditLogs
                .AsNoTracking()
                .OrderByDescending(a => a.CreatedAt)
                .Take(10)
                .Select(a => new StaffDashboardActivityResponseDto
                {
                    Action = a.Action,
                    Staff = new StaffUserSummaryResponseDto
                    {
                        Id = a.Actor.Id,
                        Username = a.Actor.Username
                    },
                    Target = a.EntityType + ": " + a.EntityId,
                    PerformedAt = a.CreatedAt
                })
                .ToListAsync(ct);

            return new StaffDashboardResponseDto
            {
                PendingNovels = pendingNovels,
                PendingChapters = pendingChapters,
                OpenReports = openNovelReports + openUserReports,
                RecentActivity = recentActivities
            };
        }
    }
}
