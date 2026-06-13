using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private const string NovelTargetType = "Novel";
        private const string UserTargetType = "User";

        private readonly LitNovelContext _context;

        public ReportRepository(LitNovelContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<AdminReportResponseDto>> GetAdminReportsAsync(AdminReportsQueryDto query, CancellationToken ct)
        {
            var page = query.Page <= 0 ? 1 : query.Page;
            var size = query.Size <= 0 ? 20 : query.Size;
            var targetType = query.Type?.Trim().ToLowerInvariant();

            IQueryable<AdminReportRow> reports = targetType switch
            {
                "novel" => QueryNovelReports(query),
                "user" => QueryUserReports(query),
                _ => QueryNovelReports(query).Concat(QueryUserReports(query))
            };

            var total = await reports.CountAsync(ct);
            var rows = await reports
                .OrderByDescending(r => r.CreatedAt)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(ct);
            var items = rows
                .Select(r => new AdminReportResponseDto
                {
                    Id = r.Id,
                    ReportType = r.ReportType,
                    TargetType = r.TargetType,
                    TargetId = r.TargetId,
                    TargetTitle = r.TargetTitle,
                    Reporter = new AdminUserSummaryResponseDto
                    {
                        Id = r.ReporterId,
                        Username = r.ReporterUsername
                    },
                    Status = r.Status,
                    CreatedAt = r.CreatedAt
                })
                .ToList();

            return new PagedResult<AdminReportResponseDto>
            {
                Items = items,
                Page = page,
                Size = size,
                TotalElements = total,
                TotalPages = (int)Math.Ceiling(total / (double)size)
            };
        }

        private IQueryable<AdminReportRow> QueryNovelReports(AdminReportsQueryDto query)
        {
            var reports = _context.NovelReports.AsNoTracking();
            reports = ApplyCommonFilters(reports, query);

            return reports.Select(r => new AdminReportRow
            {
                Id = r.Id,
                ReportType = r.ReportType.ToString(),
                TargetType = NovelTargetType,
                TargetId = r.TargetNovelId,
                TargetTitle = r.TargetNovel.Title,
                ReporterId = r.Reporter.Id,
                ReporterUsername = r.Reporter.Username,
                Status = r.Status.ToString(),
                CreatedAt = r.CreatedAt
            });
        }

        private IQueryable<AdminReportRow> QueryUserReports(AdminReportsQueryDto query)
        {
            var reports = _context.UserReports.AsNoTracking();
            reports = ApplyCommonFilters(reports, query);

            return reports.Select(r => new AdminReportRow
            {
                Id = r.Id,
                ReportType = r.ReportType.ToString(),
                TargetType = UserTargetType,
                TargetId = r.TargetUserId,
                TargetTitle = r.TargetUser.Username,
                ReporterId = r.Reporter.Id,
                ReporterUsername = r.Reporter.Username,
                Status = r.Status.ToString(),
                CreatedAt = r.CreatedAt
            });
        }

        private static IQueryable<TReport> ApplyCommonFilters<TReport>(IQueryable<TReport> reports, AdminReportsQueryDto query)
            where TReport : Domain.Common.BaseReport
        {
            if (!string.IsNullOrWhiteSpace(query.Status)
                && Enum.TryParse<ReportStatus>(query.Status, true, out var status))
            {
                reports = reports.Where(r => r.Status == status);
            }

            if (query.FromDate.HasValue)
            {
                reports = reports.Where(r => r.CreatedAt >= query.FromDate.Value);
            }

            if (query.ToDate.HasValue)
            {
                reports = reports.Where(r => r.CreatedAt <= query.ToDate.Value);
            }

            if (query.ProcessedById.HasValue)
            {
                reports = reports.Where(r => r.ProcessedById == query.ProcessedById.Value);
            }

            return reports;
        }

        private sealed class AdminReportRow
        {
            public int Id { get; set; }
            public string ReportType { get; set; } = default!;
            public string TargetType { get; set; } = default!;
            public int TargetId { get; set; }
            public string TargetTitle { get; set; } = default!;
            public int ReporterId { get; set; }
            public string ReporterUsername { get; set; } = default!;
            public string Status { get; set; } = default!;
            public DateTime CreatedAt { get; set; }
        }
    }
}
