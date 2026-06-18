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
        private readonly IGetUserWarningsUseCase      _getUserWarnings;
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
            IGetUserWarningsUseCase getUserWarnings,
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
            _getUserWarnings      = getUserWarnings;
            _getModerationHistory = getModerationHistory;
        }

        // ─────────────────────────────────────────────
        // SCR-38: GET /api/staff/dashboard
        // ─────────────────────────────────────────────
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard(CancellationToken ct)
        {
            var result = await _getDashboard.ExecuteAsync(ct);
            return Ok(new ApiResponse<StaffDashboardResponseDto> { Success = true, Data = result });
        }

        // ─────────────────────────────────────────────
        // SCR-39: Pending Novels
        // ─────────────────────────────────────────────

        /// <summary>GET /api/staff/novels/pending?page=1&amp;size=20</summary>
        [HttpGet("novels/pending")]
        public async Task<IActionResult> GetPendingNovels(
            [FromQuery] int page = 1, [FromQuery] int size = 20, CancellationToken ct = default)
        {
            var result = await _getPendingNovels.ExecuteAsync(page, size, ct);
            return Ok(new ApiResponse<PagedResult<PendingNovelListItemResponseDto>> { Success = true, Data = result });
        }

        // ─────────────────────────────────────────────
        // SCR-40: Novel Review Detail
        // ─────────────────────────────────────────────

        /// <summary>GET /api/staff/novels/{id} — full novel for review</summary>
        [HttpGet("novels/{id:int}")]
        public async Task<IActionResult> GetNovelForReview(int id, CancellationToken ct)
        {
            var result = await _getNovelForReview.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<NovelReviewDetailResponseDto> { Success = true, Data = result });
        }

        /// <summary>POST /api/staff/novels/{id}/approve</summary>
        [HttpPost("novels/{id:int}/approve")]
        public async Task<IActionResult> ApproveNovel(int id, CancellationToken ct)
        {
            await _moderateNovel.ExecuteAsync(id, new ModerateNovelRequestDto { Action = "Approve" }, ct);
            return Ok(new ApiResponse<object> { Success = true, Message = "Novel approved.", Data = null });
        }

        /// <summary>POST /api/staff/novels/{id}/reject</summary>
        [HttpPost("novels/{id:int}/reject")]
        public async Task<IActionResult> RejectNovel(int id, [FromBody] RejectReasonDto body, CancellationToken ct)
        {
            await _moderateNovel.ExecuteAsync(id, new ModerateNovelRequestDto { Action = "Reject", Reason = body.Reason }, ct);
            return Ok(new ApiResponse<object> { Success = true, Message = "Novel rejected.", Data = null });
        }

        /// <summary>POST /api/staff/novels/{id}/lock</summary>
        [HttpPost("novels/{id:int}/lock")]
        public async Task<IActionResult> LockNovel(int id, [FromBody] RejectReasonDto body, CancellationToken ct)
        {
            await _moderateNovel.ExecuteAsync(id, new ModerateNovelRequestDto { Action = "Lock", Reason = body.Reason }, ct);
            return Ok(new ApiResponse<object> { Success = true, Message = "Novel locked.", Data = null });
        }

        /// <summary>PUT /api/staff/novels/{id}/moderate — backward-compatible unified endpoint</summary>
        [HttpPut("novels/{id:int}/moderate")]
        public async Task<IActionResult> ModerateNovel(int id, [FromBody] ModerateNovelRequestDto request, CancellationToken ct)
        {
            await _moderateNovel.ExecuteAsync(id, request, ct);
            return Ok(new ApiResponse<object> { Success = true, Message = $"Novel {request.Action}d successfully.", Data = null });
        }

        // ─────────────────────────────────────────────
        // SCR-41: Pending Chapters
        // ─────────────────────────────────────────────

        /// <summary>GET /api/staff/chapters/pending?page=1&amp;size=20</summary>
        [HttpGet("chapters/pending")]
        public async Task<IActionResult> GetPendingChapters(
            [FromQuery] int page = 1, [FromQuery] int size = 20, CancellationToken ct = default)
        {
            var result = await _getPendingChapters.ExecuteAsync(page, size, ct);
            return Ok(new ApiResponse<PagedResult<PendingChapterListItemResponseDto>> { Success = true, Data = result });
        }

        // ─────────────────────────────────────────────
        // SCR-42: Chapter Review Detail
        // ─────────────────────────────────────────────

        /// <summary>GET /api/staff/chapters/{id} — full chapter content for review</summary>
        [HttpGet("chapters/{id:int}")]
        public async Task<IActionResult> GetChapterForReview(int id, CancellationToken ct)
        {
            var result = await _getChapterForReview.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<ChapterReviewDetailResponseDto> { Success = true, Data = result });
        }

        /// <summary>POST /api/staff/chapters/{id}/approve</summary>
        [HttpPost("chapters/{id:int}/approve")]
        public async Task<IActionResult> ApproveChapter(int id, CancellationToken ct)
        {
            await _moderateChapter.ExecuteAsync(id, new ModerateChapterRequestDto { Action = "Approve" }, ct);
            return Ok(new ApiResponse<object> { Success = true, Message = "Chapter approved and published.", Data = null });
        }

        /// <summary>POST /api/staff/chapters/{id}/reject</summary>
        [HttpPost("chapters/{id:int}/reject")]
        public async Task<IActionResult> RejectChapter(int id, [FromBody] RejectReasonDto body, CancellationToken ct)
        {
            await _moderateChapter.ExecuteAsync(id, new ModerateChapterRequestDto { Action = "Reject", Reason = body.Reason }, ct);
            return Ok(new ApiResponse<object> { Success = true, Message = "Chapter rejected.", Data = null });
        }

        /// <summary>POST /api/staff/chapters/{id}/lock</summary>
        [HttpPost("chapters/{id:int}/lock")]
        public async Task<IActionResult> LockChapter(int id, [FromBody] RejectReasonDto body, CancellationToken ct)
        {
            await _moderateChapter.ExecuteAsync(id, new ModerateChapterRequestDto { Action = "Lock", Reason = body.Reason }, ct);
            return Ok(new ApiResponse<object> { Success = true, Message = "Chapter locked.", Data = null });
        }

        /// <summary>PUT /api/staff/chapters/{id}/moderate — backward-compatible</summary>
        [HttpPut("chapters/{id:int}/moderate")]
        public async Task<IActionResult> ModerateChapter(int id, [FromBody] ModerateChapterRequestDto request, CancellationToken ct)
        {
            await _moderateChapter.ExecuteAsync(id, request, ct);
            return Ok(new ApiResponse<object> { Success = true, Message = $"Chapter {request.Action}d successfully.", Data = null });
        }

        // ─────────────────────────────────────────────
        // SCR-43: Reports Center
        // ─────────────────────────────────────────────

        /// <summary>GET /api/staff/reports?type=novel&amp;status=Pending&amp;page=1&amp;size=20</summary>
        [HttpGet("reports")]
        public async Task<IActionResult> GetReports(
            [FromQuery] string? type   = null,
            [FromQuery] string? status = null,
            [FromQuery] int     page   = 1,
            [FromQuery] int     size   = 20,
            CancellationToken ct = default)
        {
            var result = await _getReports.ExecuteAsync(type, status, page, size, ct);
            return Ok(new ApiResponse<PagedResult<ReportListItemResponseDto>> { Success = true, Data = result });
        }

        // ─────────────────────────────────────────────
        // SCR-44: Report Detail
        // ─────────────────────────────────────────────

        /// <summary>GET /api/staff/reports/{id}?kind=Novel</summary>
        [HttpGet("reports/{id:int}")]
        public async Task<IActionResult> GetReportDetail(int id, [FromQuery] string kind, CancellationToken ct)
        {
            var result = await _getReportDetail.ExecuteAsync(id, kind, ct);
            return Ok(new ApiResponse<ReportDetailResponseDto> { Success = true, Data = result });
        }

        /// <summary>POST /api/staff/reports/{id}/resolve?kind=Novel</summary>
        [HttpPost("reports/{id:int}/resolve")]
        public async Task<IActionResult> ResolveReport(
            int id, [FromQuery] string kind,
            [FromBody] ResolveReportRequestDto request,
            CancellationToken ct)
        {
            request.Action = "Resolve";
            await _resolveReport.ExecuteAsync(id, kind, request, ct);
            return Ok(new ApiResponse<object> { Success = true, Message = "Report resolved.", Data = null });
        }

        /// <summary>POST /api/staff/reports/{id}/reject-report?kind=Novel</summary>
        [HttpPost("reports/{id:int}/reject-report")]
        public async Task<IActionResult> RejectReport(
            int id, [FromQuery] string kind,
            [FromBody] ResolveReportRequestDto request,
            CancellationToken ct)
        {
            request.Action = "Reject";
            await _resolveReport.ExecuteAsync(id, kind, request, ct);
            return Ok(new ApiResponse<object> { Success = true, Message = "Report rejected.", Data = null });
        }

        /// <summary>PUT /api/staff/reports/{id}/resolve — backward-compatible</summary>
        [HttpPut("reports/{id:int}/resolve")]
        public async Task<IActionResult> ResolveReportLegacy(
            int id, [FromQuery] string kind,
            [FromBody] ResolveReportRequestDto request,
            CancellationToken ct)
        {
            await _resolveReport.ExecuteAsync(id, kind, request, ct);
            return Ok(new ApiResponse<object> { Success = true, Message = "Report processed.", Data = null });
        }

        // ─────────────────────────────────────────────
        // SCR-45: User Warning
        // ─────────────────────────────────────────────

        /// <summary>POST /api/staff/users/{id}/warn</summary>
        [HttpPost("users/{id:int}/warn")]
        public async Task<IActionResult> WarnUser(int id, [FromBody] WarnUserRequestDto request, CancellationToken ct)
        {
            await _warnUser.ExecuteAsync(id, request, ct);
            return Ok(new ApiResponse<object> { Success = true, Message = "Warning issued successfully.", Data = null });
        }

        /// <summary>GET /api/staff/users/{id}/warnings?page=1&amp;size=20</summary>
        [HttpGet("users/{id:int}/warnings")]
        public async Task<IActionResult> GetUserWarnings(
            int id,
            [FromQuery] int page = 1,
            [FromQuery] int size = 20,
            CancellationToken ct = default)
        {
            var result = await _getUserWarnings.ExecuteAsync(id, page, size, ct);
            return Ok(new ApiResponse<PagedResult<UserWarningResponseDto>> { Success = true, Data = result });
        }

        // ─────────────────────────────────────────────
        // SCR-46: Moderation History
        // ─────────────────────────────────────────────

        /// <summary>
        /// GET /api/staff/moderation/history
        /// ?staffId=2&amp;actionType=ApproveNovel&amp;fromDate=2024-01-01&amp;toDate=2024-01-31&amp;page=1&amp;size=20
        /// </summary>
        [HttpGet("moderation/history")]
        public async Task<IActionResult> GetModerationHistory(
            [FromQuery] int?      staffId    = null,
            [FromQuery] string?   actionType = null,
            [FromQuery] DateTime? fromDate   = null,
            [FromQuery] DateTime? toDate     = null,
            [FromQuery] int       page       = 1,
            [FromQuery] int       size       = 20,
            CancellationToken ct = default)
        {
            var result = await _getModerationHistory.ExecuteAsync(staffId, actionType, fromDate, toDate, page, size, ct);
            return Ok(new ApiResponse<PagedResult<ModerationHistoryItemResponseDto>> { Success = true, Data = result });
        }

        /// <summary>GET /api/staff/history — backward-compatible alias</summary>
        [HttpGet("history")]
        public async Task<IActionResult> GetHistoryAlias(
            [FromQuery] int page = 1, [FromQuery] int size = 20, CancellationToken ct = default)
        {
            var result = await _getModerationHistory.ExecuteAsync(null, null, null, null, page, size, ct);
            return Ok(new ApiResponse<PagedResult<ModerationHistoryItemResponseDto>> { Success = true, Data = result });
        }
    }

    /// <summary>Simple reason body for reject/lock endpoints.</summary>
    public record RejectReasonDto(string Reason);
}
