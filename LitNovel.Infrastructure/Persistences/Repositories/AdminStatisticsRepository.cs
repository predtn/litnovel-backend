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

        public async Task<AdminStatisticsChartResponseDto> GetStatisticsChartAsync(
            AdminStatisticsChartQueryDto query,
            CancellationToken ct)
        {
            var from = query.From!.Value.Date;
            var to = query.To!.Value.Date;
            var toExclusive = to.AddDays(1);
            var granularity = query.Granularity.ToLowerInvariant();

            var dates = await GetMetricDatesAsync(query.Metric, from, toExclusive, ct);
            var counts = dates
                .GroupBy(date => GetBucketStart(date, granularity))
                .ToDictionary(group => group.Key, group => group.Count());

            return new AdminStatisticsChartResponseDto
            {
                Metric = query.Metric,
                Points = BuildPoints(from, to, granularity, counts)
            };
        }

        private async Task<List<DateTime>> GetMetricDatesAsync(
            string metric,
            DateTime from,
            DateTime toExclusive,
            CancellationToken ct)
        {
            return metric switch
            {
                "userGrowth" => await _context.Users
                    .AsNoTracking()
                    .Where(u => u.CreatedAt >= from && u.CreatedAt < toExclusive)
                    .Select(u => u.CreatedAt)
                    .ToListAsync(ct),
                "novelGrowth" => await _context.Novels
                    .AsNoTracking()
                    .Where(n => n.CreatedAt >= from && n.CreatedAt < toExclusive)
                    .Select(n => n.CreatedAt)
                    .ToListAsync(ct),
                "chapterPublished" => await _context.Chapters
                    .AsNoTracking()
                    .Where(c => c.Status == ChapterStatus.Published && c.UpdatedAt >= from && c.UpdatedAt < toExclusive)
                    .Select(c => c.UpdatedAt)
                    .ToListAsync(ct),
                "comments" => await _context.CommentChapters
                    .AsNoTracking()
                    .Where(c => c.CreatedAt >= from && c.CreatedAt < toExclusive)
                    .Select(c => c.CreatedAt)
                    .ToListAsync(ct),
                "ratings" => await _context.NovelRatings
                    .AsNoTracking()
                    .Where(r => r.CreatedAt >= from && r.CreatedAt < toExclusive)
                    .Select(r => r.CreatedAt)
                    .ToListAsync(ct),
                "favorites" => await _context.Favorites
                    .AsNoTracking()
                    .Where(f => f.CreatedAt >= from && f.CreatedAt < toExclusive)
                    .Select(f => f.CreatedAt)
                    .ToListAsync(ct),
                "reports" => await GetReportDatesAsync(from, toExclusive, ct),
                _ => new List<DateTime>()
            };
        }

        private async Task<List<DateTime>> GetReportDatesAsync(DateTime from, DateTime toExclusive, CancellationToken ct)
        {
            var novelReportDates = await _context.NovelReports
                .AsNoTracking()
                .Where(r => r.CreatedAt >= from && r.CreatedAt < toExclusive)
                .Select(r => r.CreatedAt)
                .ToListAsync(ct);

            var userReportDates = await _context.UserReports
                .AsNoTracking()
                .Where(r => r.CreatedAt >= from && r.CreatedAt < toExclusive)
                .Select(r => r.CreatedAt)
                .ToListAsync(ct);

            novelReportDates.AddRange(userReportDates);
            return novelReportDates;
        }

        private static IReadOnlyList<AdminStatisticsChartPointResponseDto> BuildPoints(
            DateTime from,
            DateTime to,
            string granularity,
            IReadOnlyDictionary<DateTime, int> counts)
        {
            var points = new List<AdminStatisticsChartPointResponseDto>();
            var cursor = GetBucketStart(from, granularity);
            var end = GetBucketStart(to, granularity);

            while (cursor <= end)
            {
                points.Add(new AdminStatisticsChartPointResponseDto
                {
                    Date = cursor.ToString("yyyy-MM-dd"),
                    Value = counts.TryGetValue(cursor, out var value) ? value : 0
                });

                cursor = granularity == "month" ? cursor.AddMonths(1) : cursor.AddDays(1);
            }

            return points;
        }

        private static DateTime GetBucketStart(DateTime date, string granularity)
        {
            return granularity == "month"
                ? new DateTime(date.Year, date.Month, 1)
                : date.Date;
        }
    }
}
