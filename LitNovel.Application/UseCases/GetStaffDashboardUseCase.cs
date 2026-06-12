using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.UseCases
{
    public class GetStaffDashboardUseCase : IGetStaffDashboardUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly IChapterRepository _chapterRepository;
        private readonly INovelReportRepository _novelReportRepository;
        private readonly IUserReportRepository _userReportRepository;

        public GetStaffDashboardUseCase(
            INovelRepository novelRepository,
            IChapterRepository chapterRepository,
            INovelReportRepository novelReportRepository,
            IUserReportRepository userReportRepository)
        {
            _novelRepository = novelRepository;
            _chapterRepository = chapterRepository;
            _novelReportRepository = novelReportRepository;
            _userReportRepository = userReportRepository;
        }

        public async Task<StaffDashboardResponseDto> ExecuteAsync(CancellationToken ct)
        {
            // ⚠️ EF Core DbContext is NOT thread-safe.
            // Task.WhenAll() gây lỗi "A second operation was started on this context".
            // Phải await tuần tự.
            var pendingNovels   = await _novelRepository.CountPendingAsync(ct);
            var pendingChapters = await _chapterRepository.CountPendingAsync(ct);
            var novelReports    = await _novelReportRepository.CountPendingAsync(ct);
            var userReports     = await _userReportRepository.CountPendingAsync(ct);

            return new StaffDashboardResponseDto
            {
                PendingNovels   = pendingNovels,
                PendingChapters = pendingChapters,
                OpenReports     = novelReports + userReports
            };
        }
    }
}
