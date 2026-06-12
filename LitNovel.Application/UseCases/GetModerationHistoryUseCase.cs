using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Staff;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class GetModerationHistoryUseCase : IGetModerationHistoryUseCase
    {
        private readonly INovelReportRepository _novelReportRepository;
        private readonly IUserReportRepository  _userReportRepository;
        private readonly ICurrentUserService    _currentUserService;

        public GetModerationHistoryUseCase(
            INovelReportRepository novelReportRepository,
            IUserReportRepository userReportRepository,
            ICurrentUserService currentUserService)
        {
            _novelReportRepository = novelReportRepository;
            _userReportRepository  = userReportRepository;
            _currentUserService    = currentUserService;
        }

        public async Task<PagedResult<ModerationHistoryItemResponseDto>> ExecuteAsync(int page, int size, CancellationToken ct)
        {
            var staffId = _currentUserService.UserId;
            if (staffId <= 0) throw new UnauthorizedException("Not authenticated.");

            var safePage = page <= 0 ? 1 : page;
            var safeSize = size <= 0 ? 20 : Math.Min(size, 100);

            // Fetch đủ records từ cả 2 repo (luôn từ page 1) để merge & re-paginate trong memory
            var fetchSize = Math.Min(safeSize * 10, 200); // lấy nhiều hơn cần, tối đa 200
            var novelHistory = await _novelReportRepository.GetListAsync("Resolved,Rejected", 1, fetchSize, ct);
            var userHistory  = await _userReportRepository.GetListAsync("Resolved,Rejected", 1, fetchSize, ct);

            var allItems = novelHistory.Items
                .Select(r => new ModerationHistoryItemResponseDto
                {
                    Id              = r.Id,
                    Kind            = "NovelReport",
                    Action          = r.Status,
                    TargetTitle     = r.TargetNovel?.Title ?? "—",
                    ResolutionNotes = null,
                    ProcessedAt     = r.CreatedAt
                })
                .Concat(userHistory.Items.Select(r => new ModerationHistoryItemResponseDto
                {
                    Id              = r.Id,
                    Kind            = "UserReport",
                    Action          = r.Status,
                    TargetTitle     = r.TargetUser?.Username ?? "—",
                    ResolutionNotes = null,
                    ProcessedAt     = r.CreatedAt
                }))
                .OrderByDescending(r => r.ProcessedAt)
                .Skip((safePage - 1) * safeSize)
                .Take(safeSize)
                .ToList();

            var total = novelHistory.TotalElements + userHistory.TotalElements;

            return new PagedResult<ModerationHistoryItemResponseDto>
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
