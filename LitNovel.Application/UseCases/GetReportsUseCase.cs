using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.UseCases
{
    public class GetReportsUseCase : IGetReportsUseCase
    {
        private readonly INovelReportRepository _novelReportRepository;
        private readonly IUserReportRepository _userReportRepository;

        public GetReportsUseCase(
            INovelReportRepository novelReportRepository,
            IUserReportRepository userReportRepository)
        {
            _novelReportRepository = novelReportRepository;
            _userReportRepository  = userReportRepository;
        }

        public async Task<PagedResult<ReportListItemResponseDto>> ExecuteAsync(
            string? kind, string? status, int page, int size, CancellationToken ct)
        {
            var safePage = page <= 0 ? 1 : page;
            var safeSize = size <= 0 ? 20 : Math.Min(size, 100);

            var normalizedKind = kind?.Trim().ToLowerInvariant();

            if (normalizedKind == "novel")
                return await _novelReportRepository.GetListAsync(status, safePage, safeSize, ct);

            if (normalizedKind == "user")
                return await _userReportRepository.GetListAsync(status, safePage, safeSize, ct);

            // ⚠️ EF Core DbContext không thread-safe — await tuần tự
            var novelResult = await _novelReportRepository.GetListAsync(status, safePage, safeSize, ct);
            var userResult  = await _userReportRepository.GetListAsync(status, safePage, safeSize, ct);

            var allItems = novelResult.Items
                .Concat(userResult.Items)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((safePage - 1) * safeSize)
                .Take(safeSize)
                .ToList();

            var total = novelResult.TotalElements + userResult.TotalElements;

            return new PagedResult<ReportListItemResponseDto>
            {
                Items         = allItems,
                Page          = safePage,
                Size          = safeSize,
                TotalElements = total,
                TotalPages    = (int)Math.Ceiling(total / (double)safeSize)
            };
        }
    }
}
