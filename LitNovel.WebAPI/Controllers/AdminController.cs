using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Admin;
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
        private readonly IUpdateAdminUserUseCase _updateAdminUserUseCase;
        private readonly IBanAdminUserUseCase _banAdminUserUseCase;
        private readonly IUnbanAdminUserUseCase _unbanAdminUserUseCase;
        private readonly IDeleteAdminUserUseCase _deleteAdminUserUseCase;

        public AdminController(
            IGetAdminStatisticsUseCase getAdminStatisticsUseCase,
            IGetAdminUsersUseCase getAdminUsersUseCase,
            IUpdateAdminUserUseCase updateAdminUserUseCase,
            IBanAdminUserUseCase banAdminUserUseCase,
            IUnbanAdminUserUseCase unbanAdminUserUseCase,
            IDeleteAdminUserUseCase deleteAdminUserUseCase)
        {
            _getAdminStatisticsUseCase = getAdminStatisticsUseCase;
            _getAdminUsersUseCase = getAdminUsersUseCase;
            _updateAdminUserUseCase = updateAdminUserUseCase;
            _banAdminUserUseCase = banAdminUserUseCase;
            _unbanAdminUserUseCase = unbanAdminUserUseCase;
            _deleteAdminUserUseCase = deleteAdminUserUseCase;
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

        [HttpDelete("users/{id:int}")]
        public async Task<IActionResult> DeleteUser(int id, CancellationToken ct)
        {
            await _deleteAdminUserUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<object> { Success = true, Data = null });
        }
    }
}
