using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Application.DTOs.Category;
using LitNovel.Application.DTOs.Tag;
using LitNovel.WebAPI.Common;
using LitNovel.WebAPI.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace LitNovel.WebAPI.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IGetAdminStatisticsUseCase _getAdminStatisticsUseCase;
        private readonly IGetAdminStatisticsChartUseCase _getAdminStatisticsChartUseCase;
        private readonly IGetAdminUsersUseCase _getAdminUsersUseCase;
        private readonly IGetAdminUserDetailUseCase _getAdminUserDetailUseCase;
        private readonly IUpdateAdminUserUseCase _updateAdminUserUseCase;
        private readonly IBanAdminUserUseCase _banAdminUserUseCase;
        private readonly IUnbanAdminUserUseCase _unbanAdminUserUseCase;
        private readonly IDeleteAdminUserUseCase _deleteAdminUserUseCase;
        private readonly IAssignStaffUseCase _assignStaffUseCase;
        private readonly IRevokeStaffUseCase _revokeStaffUseCase;
        private readonly IGetAdminBadgesUseCase _getAdminBadgesUseCase;
        private readonly ICreateAdminBadgeUseCase _createAdminBadgeUseCase;
        private readonly IUpdateAdminBadgeUseCase _updateAdminBadgeUseCase;
        private readonly IDeleteAdminBadgeUseCase _deleteAdminBadgeUseCase;
        private readonly IAwardBadgeUseCase _awardBadgeUseCase;
        private readonly IGetAdminCategoriesUseCase _getAdminCategoriesUseCase;
        private readonly ICreateAdminCategoryUseCase _createAdminCategoryUseCase;
        private readonly IUpdateAdminCategoryUseCase _updateAdminCategoryUseCase;
        private readonly IDeleteAdminCategoryUseCase _deleteAdminCategoryUseCase;
        private readonly IGetAdminTagsUseCase _getAdminTagsUseCase;
        private readonly ICreateAdminTagUseCase _createAdminTagUseCase;
        private readonly IUpdateAdminTagUseCase _updateAdminTagUseCase;
        private readonly IDeleteAdminTagUseCase _deleteAdminTagUseCase;
        private readonly IGetAdminSentNotificationsUseCase _getAdminSentNotificationsUseCase;
        private readonly ISendAdminNotificationUseCase _sendAdminNotificationUseCase;
        private readonly IGetAdminReportsUseCase _getAdminReportsUseCase;
        private readonly IGetAdminAuditLogsUseCase _getAdminAuditLogsUseCase;
        private readonly IUpdateAdminNovelStatusUseCase _updateAdminNovelStatusUseCase;
        private readonly IUpdateAdminNovelAuthorUseCase _updateAdminNovelAuthorUseCase;
        private readonly IUpdateAdminChapterStatusUseCase _updateAdminChapterStatusUseCase;

        public AdminController(
            IGetAdminStatisticsUseCase getAdminStatisticsUseCase,
            IGetAdminStatisticsChartUseCase getAdminStatisticsChartUseCase,
            IGetAdminUsersUseCase getAdminUsersUseCase,
            IGetAdminUserDetailUseCase getAdminUserDetailUseCase,
            IUpdateAdminUserUseCase updateAdminUserUseCase,
            IBanAdminUserUseCase banAdminUserUseCase,
            IUnbanAdminUserUseCase unbanAdminUserUseCase,
            IDeleteAdminUserUseCase deleteAdminUserUseCase,
            IAssignStaffUseCase assignStaffUseCase,
            IRevokeStaffUseCase revokeStaffUseCase,
            IGetAdminBadgesUseCase getAdminBadgesUseCase,
            ICreateAdminBadgeUseCase createAdminBadgeUseCase,
            IUpdateAdminBadgeUseCase updateAdminBadgeUseCase,
            IDeleteAdminBadgeUseCase deleteAdminBadgeUseCase,
            IAwardBadgeUseCase awardBadgeUseCase,
            IGetAdminCategoriesUseCase getAdminCategoriesUseCase,
            ICreateAdminCategoryUseCase createAdminCategoryUseCase,
            IUpdateAdminCategoryUseCase updateAdminCategoryUseCase,
            IDeleteAdminCategoryUseCase deleteAdminCategoryUseCase,
            IGetAdminTagsUseCase getAdminTagsUseCase,
            ICreateAdminTagUseCase createAdminTagUseCase,
            IUpdateAdminTagUseCase updateAdminTagUseCase,
            IDeleteAdminTagUseCase deleteAdminTagUseCase,
            IGetAdminSentNotificationsUseCase getAdminSentNotificationsUseCase,
            ISendAdminNotificationUseCase sendAdminNotificationUseCase,
            IGetAdminReportsUseCase getAdminReportsUseCase,
            IGetAdminAuditLogsUseCase getAdminAuditLogsUseCase,
            IUpdateAdminNovelStatusUseCase updateAdminNovelStatusUseCase,
            IUpdateAdminNovelAuthorUseCase updateAdminNovelAuthorUseCase,
            IUpdateAdminChapterStatusUseCase updateAdminChapterStatusUseCase)
        {
            _getAdminStatisticsUseCase = getAdminStatisticsUseCase;
            _getAdminStatisticsChartUseCase = getAdminStatisticsChartUseCase;
            _getAdminUsersUseCase = getAdminUsersUseCase;
            _getAdminUserDetailUseCase = getAdminUserDetailUseCase;
            _updateAdminUserUseCase = updateAdminUserUseCase;
            _banAdminUserUseCase = banAdminUserUseCase;
            _unbanAdminUserUseCase = unbanAdminUserUseCase;
            _deleteAdminUserUseCase = deleteAdminUserUseCase;
            _assignStaffUseCase = assignStaffUseCase;
            _revokeStaffUseCase = revokeStaffUseCase;
            _getAdminBadgesUseCase = getAdminBadgesUseCase;
            _createAdminBadgeUseCase = createAdminBadgeUseCase;
            _updateAdminBadgeUseCase = updateAdminBadgeUseCase;
            _deleteAdminBadgeUseCase = deleteAdminBadgeUseCase;
            _awardBadgeUseCase = awardBadgeUseCase;
            _getAdminCategoriesUseCase = getAdminCategoriesUseCase;
            _createAdminCategoryUseCase = createAdminCategoryUseCase;
            _updateAdminCategoryUseCase = updateAdminCategoryUseCase;
            _deleteAdminCategoryUseCase = deleteAdminCategoryUseCase;
            _getAdminTagsUseCase = getAdminTagsUseCase;
            _createAdminTagUseCase = createAdminTagUseCase;
            _updateAdminTagUseCase = updateAdminTagUseCase;
            _deleteAdminTagUseCase = deleteAdminTagUseCase;
            _getAdminSentNotificationsUseCase = getAdminSentNotificationsUseCase;
            _sendAdminNotificationUseCase = sendAdminNotificationUseCase;
            _getAdminReportsUseCase = getAdminReportsUseCase;
            _getAdminAuditLogsUseCase = getAdminAuditLogsUseCase;
            _updateAdminNovelStatusUseCase = updateAdminNovelStatusUseCase;
            _updateAdminNovelAuthorUseCase = updateAdminNovelAuthorUseCase;
            _updateAdminChapterStatusUseCase = updateAdminChapterStatusUseCase;
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics(CancellationToken ct)
        {
            var result = await _getAdminStatisticsUseCase.ExecuteAsync(ct);
            return Ok(new ApiResponse<AdminStatisticsResponseDto> { Success = true, Data = result });
        }

        [HttpGet("statistics/chart")]
        public async Task<IActionResult> GetStatisticsChart([FromQuery] AdminStatisticsChartQueryDto query, CancellationToken ct)
        {
            var result = await _getAdminStatisticsChartUseCase.ExecuteAsync(query, ct);
            return Ok(new ApiResponse<AdminStatisticsChartResponseDto> { Success = true, Data = result });
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers(ODataQueryOptions<AdminUserListItemResponseDto> queryOptions, CancellationToken ct)
        {
            var result = await ODataQueryResultFactory.ToPagedResultAsync(
                _getAdminUsersUseCase.ExecuteQuery(),
                queryOptions,
                users => users.OrderByDescending(u => u.JoinedAt),
                defaultPageSize: 20,
                maxTop: 100,
                ct);

            return Ok(new ApiResponse<PagedResult<AdminUserListItemResponseDto>> { Success = true, Data = result });
        }

        [HttpGet("users/{id:int}")]
        public async Task<IActionResult> GetUserDetail(int id, CancellationToken ct)
        {
            var result = await _getAdminUserDetailUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<AdminUserDetailResponseDto> { Success = true, Data = result });
        }

        [HttpPut("users/{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateAdminUserRequestDto request, CancellationToken ct)
        {
            var result = await _updateAdminUserUseCase.ExecuteAsync(id, request, ct);
            return Ok(new ApiResponse<AdminUserUpdateResponseDto>
            {
                Success = true,
                Message = "User updated successfully",
                Data = result
            });
        }

        [HttpPost("users/{id:int}/ban")]
        public async Task<IActionResult> BanUser(int id, BanAdminUserRequestDto request, CancellationToken ct)
        {
            var result = await _banAdminUserUseCase.ExecuteAsync(id, request, ct);
            return Ok(new ApiResponse<BanAdminUserResponseDto>
            {
                Success = true,
                Message = "User has been banned",
                Data = result
            });
        }

        [HttpPost("users/{id:int}/unban")]
        public async Task<IActionResult> UnbanUser(int id, CancellationToken ct)
        {
            var result = await _unbanAdminUserUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<UnbanAdminUserResponseDto>
            {
                Success = true,
                Message = "User has been unbanned",
                Data = result
            });
        }

        [HttpPost("users/{id:int}/assign-staff")]
        public async Task<IActionResult> AssignStaff(int id, CancellationToken ct)
        {
            var result = await _assignStaffUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<StaffRoleChangeResponseDto>
            {
                Success = true,
                Message = "User promoted to Staff",
                Data = result
            });
        }

        [HttpPost("users/{id:int}/revoke-staff")]
        public async Task<IActionResult> RevokeStaff(int id, CancellationToken ct)
        {
            var result = await _revokeStaffUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<StaffRoleChangeResponseDto>
            {
                Success = true,
                Message = "User demoted to User",
                Data = result
            });
        }

        [HttpDelete("users/{id:int}")]
        public async Task<IActionResult> DeleteUser(int id, CancellationToken ct)
        {
            await _deleteAdminUserUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<object> { Success = true, Data = null });
        }

        [HttpGet("badges")]
        public async Task<IActionResult> GetBadges(CancellationToken ct)
        {
            var result = await _getAdminBadgesUseCase.ExecuteAsync(ct);
            return Ok(new ApiResponse<IReadOnlyList<AdminBadgeResponseDto>> { Success = true, Data = result });
        }

        [HttpPost("badges")]
        public async Task<IActionResult> CreateBadge(CreateAdminBadgeRequestDto request, CancellationToken ct)
        {
            var result = await _createAdminBadgeUseCase.ExecuteAsync(request, ct);
            return StatusCode(StatusCodes.Status201Created, new ApiResponse<AdminBadgeResponseDto>
            {
                Success = true,
                Message = "Badge created",
                Data = result
            });
        }

        [HttpPut("badges/{id:int}")]
        public async Task<IActionResult> UpdateBadge(int id, UpdateAdminBadgeRequestDto request, CancellationToken ct)
        {
            var result = await _updateAdminBadgeUseCase.ExecuteAsync(id, request, ct);
            return Ok(new ApiResponse<AdminBadgeResponseDto>
            {
                Success = true,
                Message = "Badge updated",
                Data = result
            });
        }

        [HttpDelete("badges/{id:int}")]
        public async Task<IActionResult> DeleteBadge(int id, CancellationToken ct)
        {
            await _deleteAdminBadgeUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<object> { Success = true, Data = null });
        }

        [HttpPost("badges/{id:int}/award/{userId:int}")]
        public async Task<IActionResult> AwardBadge(int id, int userId, CancellationToken ct)
        {
            var result = await _awardBadgeUseCase.ExecuteAsync(id, userId, ct);
            return Ok(new ApiResponse<AwardBadgeResponseDto>
            {
                Success = true,
                Message = "Badge awarded successfully",
                Data = result
            });
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories(CancellationToken ct)
        {
            var result = await _getAdminCategoriesUseCase.ExecuteAsync(ct);
            return Ok(new ApiResponse<IReadOnlyList<CategoryResponseDto>> { Success = true, Data = result });
        }

        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request, CancellationToken ct)
        {
            var result = await _createAdminCategoryUseCase.ExecuteAsync(request, ct);
            return StatusCode(StatusCodes.Status201Created, new ApiResponse<CategoryResponseDto>
            {
                Success = true,
                Message = "Category created",
                Data = result
            });
        }

        [HttpPut("categories/{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryRequestDto request, CancellationToken ct)
        {
            var result = await _updateAdminCategoryUseCase.ExecuteAsync(id, request, ct);
            return Ok(new ApiResponse<CategoryResponseDto>
            {
                Success = true,
                Message = "Category updated",
                Data = result
            });
        }

        [HttpDelete("categories/{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id, CancellationToken ct)
        {
            await _deleteAdminCategoryUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<object> { Success = true, Data = null });
        }

        [HttpGet("tags")]
        public async Task<IActionResult> GetTags(CancellationToken ct)
        {
            var result = await _getAdminTagsUseCase.ExecuteAsync(ct);
            return Ok(new ApiResponse<IReadOnlyList<TagResponseDto>> { Success = true, Data = result });
        }

        [HttpPost("tags")]
        public async Task<IActionResult> CreateTag(CreateTagRequestDto request, CancellationToken ct)
        {
            var result = await _createAdminTagUseCase.ExecuteAsync(request, ct);
            return StatusCode(StatusCodes.Status201Created, new ApiResponse<TagResponseDto>
            {
                Success = true,
                Message = "Tag created",
                Data = result
            });
        }

        [HttpPut("tags/{id:int}")]
        public async Task<IActionResult> UpdateTag(int id, UpdateTagRequestDto request, CancellationToken ct)
        {
            var result = await _updateAdminTagUseCase.ExecuteAsync(id, request, ct);
            return Ok(new ApiResponse<TagResponseDto>
            {
                Success = true,
                Message = "Tag updated",
                Data = result
            });
        }

        [HttpDelete("tags/{id:int}")]
        public async Task<IActionResult> DeleteTag(int id, CancellationToken ct)
        {
            await _deleteAdminTagUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<object> { Success = true, Data = null });
        }

        [HttpGet("notifications/sent")]
        public async Task<IActionResult> GetSentNotifications(
            ODataQueryOptions<AdminSentNotificationResponseDto> queryOptions,
            [FromQuery] AdminSentNotificationsQueryDto query,
            CancellationToken ct)
        {
            if (HasODataQuery(Request))
            {
                var odataResult = await ODataQueryResultFactory.ToPagedResultAsync(
                    _getAdminSentNotificationsUseCase.ExecuteQuery(),
                    queryOptions,
                    notifications => notifications.OrderByDescending(n => n.SentAt),
                    defaultPageSize: 20,
                    maxTop: 100,
                    ct);

                return Ok(new ApiResponse<PagedResult<AdminSentNotificationResponseDto>> { Success = true, Data = odataResult });
            }

            var result = await _getAdminSentNotificationsUseCase.ExecuteAsync(query, ct);
            return Ok(new ApiResponse<PagedResult<AdminSentNotificationResponseDto>> { Success = true, Data = result });
        }

        [HttpPost("notifications")]
        public async Task<IActionResult> SendNotification(SendAdminNotificationRequestDto request, CancellationToken ct)
        {
            var result = await _sendAdminNotificationUseCase.ExecuteAsync(request, ct);
            var message = request.TargetAll == true ? "Notification sent to all users" : "Notification sent to user";

            return StatusCode(StatusCodes.Status201Created, new ApiResponse<SendAdminNotificationResponseDto>
            {
                Success = true,
                Message = message,
                Data = result
            });
        }

        [HttpGet("reports")]
        public async Task<IActionResult> GetReports(
            ODataQueryOptions<AdminReportResponseDto> queryOptions,
            [FromQuery] AdminReportsQueryDto query,
            CancellationToken ct)
        {
            if (HasODataQuery(Request))
            {
                var odataResult = await ODataQueryResultFactory.ToPagedResultAsync(
                    _getAdminReportsUseCase.ExecuteQuery(),
                    queryOptions,
                    reports => reports.OrderByDescending(r => r.CreatedAt),
                    defaultPageSize: 20,
                    maxTop: 100,
                    ct);

                return Ok(new ApiResponse<PagedResult<AdminReportResponseDto>> { Success = true, Data = odataResult });
            }

            var result = await _getAdminReportsUseCase.ExecuteAsync(query, ct);
            return Ok(new ApiResponse<PagedResult<AdminReportResponseDto>> { Success = true, Data = result });
        }

        [HttpGet("audit-logs")]
        public async Task<IActionResult> GetAuditLogs(
            ODataQueryOptions<AdminAuditLogResponseDto> queryOptions,
            [FromQuery] AdminAuditLogQueryDto query,
            CancellationToken ct)
        {
            if (HasODataQuery(Request))
            {
                var odataResult = await ODataQueryResultFactory.ToPagedResultAsync(
                    _getAdminAuditLogsUseCase.ExecuteQuery(),
                    queryOptions,
                    auditLogs => auditLogs.OrderByDescending(a => a.CreatedAt),
                    defaultPageSize: 50,
                    maxTop: 100,
                    ct);

                return Ok(new ApiResponse<PagedResult<AdminAuditLogResponseDto>> { Success = true, Data = odataResult });
            }

            var result = await _getAdminAuditLogsUseCase.ExecuteAsync(query, ct);
            return Ok(new ApiResponse<PagedResult<AdminAuditLogResponseDto>> { Success = true, Data = result });
        }

        [HttpPut("novels/{id:int}/status")]
        public async Task<IActionResult> UpdateNovelStatus(int id, UpdateAdminNovelStatusRequestDto request, CancellationToken ct)
        {
            var result = await _updateAdminNovelStatusUseCase.ExecuteAsync(id, request, ct);
            return Ok(new ApiResponse<AdminNovelStatusResponseDto>
            {
                Success = true,
                Message = "Novel status updated successfully",
                Data = result
            });
        }

        [HttpPut("novels/{id:int}/author")]
        public async Task<IActionResult> UpdateNovelAuthor(int id, UpdateAdminNovelAuthorRequestDto request, CancellationToken ct)
        {
            var result = await _updateAdminNovelAuthorUseCase.ExecuteAsync(id, request, ct);
            return Ok(new ApiResponse<AdminNovelAuthorResponseDto>
            {
                Success = true,
                Message = "Novel author updated successfully",
                Data = result
            });
        }

        [HttpPut("chapters/{id:int}/status")]
        public async Task<IActionResult> UpdateChapterStatus(int id, UpdateAdminChapterStatusRequestDto request, CancellationToken ct)
        {
            var result = await _updateAdminChapterStatusUseCase.ExecuteAsync(id, request, ct);
            return Ok(new ApiResponse<AdminChapterStatusResponseDto>
            {
                Success = true,
                Message = "Chapter status updated successfully",
                Data = result
            });
        }

        private static bool HasODataQuery(HttpRequest request)
        {
            return request.Query.Keys.Any(key => key.StartsWith("$", StringComparison.Ordinal));
        }
    }
}
