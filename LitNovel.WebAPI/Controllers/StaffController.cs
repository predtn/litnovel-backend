using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Staff;
using LitNovel.WebAPI.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LitNovel.WebAPI.Controllers
{
    [ApiController]
    [Route("api/staff")]
    [Authorize(Roles = "Staff,Admin")]
    public class StaffController : ControllerBase
    {
        private readonly IGetStaffDashboardUseCase    _getDashboard;
        private readonly IGetPendingNovelsUseCase     _getPendingNovels;
        private readonly IGetNovelForReviewUseCase    _getNovelForReview;
        private readonly IModerateNovelUseCase        _moderateNovel;
        private readonly IGetPendingChaptersUseCase   _getPendingChapters;
        private readonly IGetChapterForReviewUseCase  _getChapterForReview;
        private readonly IModerateChapterUseCase      _moderateChapter;
        private readonly IGetReportsUseCase           _getReports;
        private readonly IGetReportDetailUseCase      _getReportDetail;
        private readonly IResolveReportUseCase        _resolveReport;
        private readonly IWarnUserUseCase             _warnUser;
        private readonly IGetModerationHistoryUseCase _getModerationHistory;

        public StaffController(
            IGetStaffDashboardUseCase getDashboard,
            IGetPendingNovelsUseCase getPendingNovels,
            IGetNovelForReviewUseCase getNovelForReview,
            IModerateNovelUseCase moderateNovel,
            IGetPendingChaptersUseCase getPendingChapters,
            IGetChapterForReviewUseCase getChapterForReview,
            IModerateChapterUseCase moderateChapter,
            IGetReportsUseCase getReports,
            IGetReportDetailUseCase getReportDetail,
            IResolveReportUseCase resolveReport,
            IWarnUserUseCase warnUser,
            IGetModerationHistoryUseCase getModerationHistory)
        {
            _getDashboard         = getDashboard;
            _getPendingNovels     = getPendingNovels;
            _getNovelForReview    = getNovelForReview;
            _moderateNovel        = moderateNovel;
            _getPendingChapters   = getPendingChapters;
            _getChapterForReview  = getChapterForReview;
            _moderateChapter      = moderateChapter;
            _getReports           = getReports;
            _getReportDetail      = getReportDetail;
            _resolveReport        = resolveReport;
            _warnUser             = warnUser;
            _getModerationHistory = getModerationHistory;
        }

        // GET /api/staff/dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard(CancellationToken ct)
        {
            var result = await _getDashboard.ExecuteAsync(ct);
            return Ok(new ApiResponse<StaffDashboardResponseDto> { Success = true, Data = result });
        }

        // GET /api/staff/novels/pending?page=1&size=20
        [HttpGet("novels/pending")]
        public async Task<IActionResult> GetPendingNovels(
            [FromQuery] int page = 1, [FromQuery] int size = 20, CancellationToken ct = default)
        {
            var result = await _getPendingNovels.ExecuteAsync(page, size, ct);
            return Ok(new ApiResponse<PagedResult<PendingNovelListItemResponseDto>> { Success = true, Data = result });
        }

        // GET /api/staff/novels/{id}
        [HttpGet("novels/{id:int}")]
        public async Task<IActionResult> GetNovelForReview(int id, CancellationToken ct)
        {
            var result = await _getNovelForReview.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<NovelReviewDetailResponseDto> { Success = true, Data = result });
        }

        // PUT /api/staff/novels/{id}/moderate
        [HttpPut("novels/{id:int}/moderate")]
        public async Task<IActionResult> ModerateNovel(int id, [FromBody] ModerateNovelRequestDto request, CancellationToken ct)
        {
            await _moderateNovel.ExecuteAsync(id, request, ct);
            return Ok(new ApiResponse<object> { Success = true, Message = $"Novel {request.Action}d successfully.", Data = null });
        }

        // GET /api/staff/chapters/pending?page=1&size=20
        [HttpGet("chapters/pending")]
        public async Task<IActionResult> GetPendingChapters(
            [FromQuery] int page = 1, [FromQuery] int size = 20, CancellationToken ct = default)
        {
            var result = await _getPendingChapters.ExecuteAsync(page, size, ct);
            return Ok(new ApiResponse<PagedResult<PendingChapterListItemResponseDto>> { Success = true, Data = result });
        }

        // GET /api/staff/chapters/{id}
        [HttpGet("chapters/{id:int}")]
        public async Task<IActionResult> GetChapterForReview(int id, CancellationToken ct)
        {
            var result = await _getChapterForReview.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<ChapterReviewDetailResponseDto> { Success = true, Data = result });
        }

        // PUT /api/staff/chapters/{id}/moderate
        [HttpPut("chapters/{id:int}/moderate")]
        public async Task<IActionResult> ModerateChapter(int id, [FromBody] ModerateChapterRequestDto request, CancellationToken ct)
        {
            await _moderateChapter.ExecuteAsync(id, request, ct);
            return Ok(new ApiResponse<object> { Success = true, Message = $"Chapter {request.Action}d successfully.", Data = null });
        }

        // GET /api/staff/reports?kind=Novel&status=Pending&page=1&size=20
        [HttpGet("reports")]
        public async Task<IActionResult> GetReports(
            [FromQuery] string? kind = null,
            [FromQuery] string? status = null,
            [FromQuery] int page = 1,
            [FromQuery] int size = 20,
            CancellationToken ct = default)
        {
            var result = await _getReports.ExecuteAsync(kind, status, page, size, ct);
            return Ok(new ApiResponse<PagedResult<ReportListItemResponseDto>> { Success = true, Data = result });
        }

        // GET /api/staff/reports/{id}?kind=Novel
        [HttpGet("reports/{id:int}")]
        public async Task<IActionResult> GetReportDetail(int id, [FromQuery] string kind, CancellationToken ct)
        {
            var result = await _getReportDetail.ExecuteAsync(id, kind, ct);
            return Ok(new ApiResponse<ReportDetailResponseDto> { Success = true, Data = result });
        }

        // PUT /api/staff/reports/{id}/resolve?kind=Novel
        [HttpPut("reports/{id:int}/resolve")]
        public async Task<IActionResult> ResolveReport(
            int id, [FromQuery] string kind, [FromBody] ResolveReportRequestDto request, CancellationToken ct)
        {
            await _resolveReport.ExecuteAsync(id, kind, request, ct);
            return Ok(new ApiResponse<object> { Success = true, Message = "Report processed.", Data = null });
        }

        // POST /api/staff/warn
        [HttpPost("warn")]
        public async Task<IActionResult> WarnUser([FromBody] WarnUserRequestDto request, CancellationToken ct)
        {
            await _warnUser.ExecuteAsync(request, ct);
            return Ok(new ApiResponse<object> { Success = true, Message = "Warning sent to user.", Data = null });
        }

        // GET /api/staff/history?page=1&size=20
        [HttpGet("history")]
        public async Task<IActionResult> GetModerationHistory(
            [FromQuery] int page = 1, [FromQuery] int size = 20, CancellationToken ct = default)
        {
            var result = await _getModerationHistory.ExecuteAsync(page, size, ct);
            return Ok(new ApiResponse<PagedResult<ModerationHistoryItemResponseDto>> { Success = true, Data = result });
        }
    }
}
