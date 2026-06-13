using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class AdminStatisticsRepository : IAdminStatisticsRepository
    {
        private readonly LitNovelContext _context;

        public AdminStatisticsRepository(LitNovelContext context)
        {
            _context = context;
        }

        public async Task<AdminStatisticsResponseDto> GetStatisticsAsync(CancellationToken ct)
        {
            var now = DateTime.UtcNow;
            var startOfWeek = now.Date.AddDays(-6);
            var startOfMonth = new DateTime(now.Year, now.Month, 1);

            var totalUsers = await _context.Users.AsNoTracking().CountAsync(ct);
            var newUsersThisWeek = await _context.Users.AsNoTracking().CountAsync(u => u.CreatedAt >= startOfWeek, ct);
            var bannedUsers = await _context.Users.AsNoTracking().CountAsync(u => u.Status == UserStatus.Banned, ct);

            var totalNovels = await _context.Novels.AsNoTracking().CountAsync(ct);
            var ongoingNovels = await _context.Novels.AsNoTracking().CountAsync(n => n.Status == NovelStatus.Ongoing, ct);
            var pendingNovels = await _context.Novels.AsNoTracking().CountAsync(n => n.Status == NovelStatus.Pending, ct);
            var newNovelsThisMonth = await _context.Novels.AsNoTracking().CountAsync(n => n.CreatedAt >= startOfMonth, ct);

            var totalChapters = await _context.Chapters.AsNoTracking().CountAsync(ct);
            var publishedChaptersThisWeek = await _context.Chapters.AsNoTracking().CountAsync(
                c => c.Status == ChapterStatus.Published && c.UpdatedAt >= startOfWeek,
                ct);

            var totalNovelReports = await _context.NovelReports.AsNoTracking().CountAsync(ct);
            var totalUserReports = await _context.UserReports.AsNoTracking().CountAsync(ct);
            var openNovelReports = await _context.NovelReports.AsNoTracking().CountAsync(r => r.Status == ReportStatus.Pending, ct);
            var openUserReports = await _context.UserReports.AsNoTracking().CountAsync(r => r.Status == ReportStatus.Pending, ct);
            var resolvedNovelReportsThisMonth = await _context.NovelReports.AsNoTracking().CountAsync(
                r => r.Status == ReportStatus.Resolved && r.UpdatedAt >= startOfMonth,
                ct);
            var resolvedUserReportsThisMonth = await _context.UserReports.AsNoTracking().CountAsync(
                r => r.Status == ReportStatus.Resolved && r.UpdatedAt >= startOfMonth,
                ct);

            return new AdminStatisticsResponseDto
            {
                Users = new AdminUserStatisticsResponseDto
                {
                    Total = totalUsers,
                    NewThisWeek = newUsersThisWeek,
                    Banned = bannedUsers
                },
                Novels = new AdminNovelStatisticsResponseDto
                {
                    Total = totalNovels,
                    Ongoing = ongoingNovels,
                    Pending = pendingNovels,
                    NewThisMonth = newNovelsThisMonth
                },
                Chapters = new AdminChapterStatisticsResponseDto
                {
                    Total = totalChapters,
                    PublishedThisWeek = publishedChaptersThisWeek
                },
                Reports = new AdminReportStatisticsResponseDto
                {
                    Total = totalNovelReports + totalUserReports,
                    Open = openNovelReports + openUserReports,
                    ResolvedThisMonth = resolvedNovelReportsThisMonth + resolvedUserReportsThisMonth
                },
                Engagement = new AdminEngagementStatisticsResponseDto
                {
                    TotalComments = await _context.CommentChapters.AsNoTracking().CountAsync(ct),
                    TotalRatings = await _context.NovelRatings.AsNoTracking().CountAsync(ct),
                    TotalFavorites = await _context.Favorites.AsNoTracking().CountAsync(ct)
                }
            };
        }
    }
}
