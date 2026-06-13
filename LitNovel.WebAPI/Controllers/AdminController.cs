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

        public AdminController(
            IGetAdminStatisticsUseCase getAdminStatisticsUseCase,
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
            IDeleteAdminTagUseCase deleteAdminTagUseCase)
        {
            _getAdminStatisticsUseCase = getAdminStatisticsUseCase;
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
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics(CancellationToken ct)
        {
            var result = await _getAdminStatisticsUseCase.ExecuteAsync(ct);
            return Ok(new ApiResponse<AdminStatisticsResponseDto> { Success = true, Data = result });
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
    }
}
