using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Staff;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class NovelReportRepository : INovelReportRepository
    {
        private readonly LitNovelContext _context;

        public NovelReportRepository(LitNovelContext context)
        {
            _context = context;
        }

        public async Task AddAsync(NovelReport report, CancellationToken ct)
        {
            await _context.NovelReports.AddAsync(report, ct);
        }

        public async Task<PagedResult<ReportListItemResponseDto>> GetListAsync(string? status, int page, int size, CancellationToken ct)
        {
            var query = _context.NovelReports.AsNoTracking();

            // Support comma-separated statuses e.g. "Resolved,Rejected"
            if (!string.IsNullOrWhiteSpace(status))
            {
                // Parse to enum list first — EF Core can translate List<Enum>.Contains() to SQL IN
                // but CANNOT translate r.Status.ToString() in LINQ-to-SQL
                var enumValues = status
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Where(s => Enum.TryParse<ReportStatus>(s, true, out _))
                    .Select(s => Enum.Parse<ReportStatus>(s, true))
                    .ToList();

                if (enumValues.Count == 1)
                    query = query.Where(r => r.Status == enumValues[0]);
                else if (enumValues.Count > 1)
                    query = query.Where(r => enumValues.Contains(r.Status));
            }

            var total = await query.CountAsync(ct);
            var items = await query
                .OrderByDescending(r => r.CreatedAt)
                .Skip((page - 1) * size)
                .Take(size)
                .Select(r => new ReportListItemResponseDto
                {
                    Id          = r.Id,
                    Kind        = "Novel",
                    ReportType  = r.ReportType.ToString(),
                    Description = r.Description,
                    Status      = r.Status.ToString(),
                    CreatedAt   = r.CreatedAt,
                    Reporter    = r.Reporter == null ? null : new ReportActorDto
                    {
                        Id = r.Reporter.Id, Username = r.Reporter.Username, Avatar = r.Reporter.Avatar
                    },
                    TargetNovel = r.TargetNovel == null ? null : new ReportTargetNovelDto
                    {
                        Id = r.TargetNovel.Id, Title = r.TargetNovel.Title, Slug = r.TargetNovel.Slug
                    }
                })
                .ToListAsync(ct);

            return new PagedResult<ReportListItemResponseDto>
            {
                Items         = items,
                Page          = page,
                Size          = size,
                TotalElements = total,
                TotalPages    = (int)Math.Ceiling(total / (double)size)
            };
        }

        public Task<NovelReport?> GetByIdWithDetailsAsync(int id, CancellationToken ct)
        {
            return _context.NovelReports
                .Include(r => r.Reporter)
                .Include(r => r.ProcessedBy)
                .Include(r => r.TargetNovel)
                .Include(r => r.TargetChapter)
                .AsSplitQuery()
                .FirstOrDefaultAsync(r => r.Id == id, ct);
        }

        public Task<int> CountPendingAsync(CancellationToken ct)
        {
            return _context.NovelReports.CountAsync(r => r.Status == ReportStatus.Pending, ct);
        }
    }
}
