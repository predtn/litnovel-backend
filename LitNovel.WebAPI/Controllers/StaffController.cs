using LitNovel.Application.Common.Interfaces.UseCases;
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
        private readonly IGetStaffDashboardUseCase _getStaffDashboardUseCase;

        public StaffController(IGetStaffDashboardUseCase getStaffDashboardUseCase)
        {
            _getStaffDashboardUseCase = getStaffDashboardUseCase;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard(CancellationToken ct)
        {
            var result = await _getStaffDashboardUseCase.ExecuteAsync(ct);
            return Ok(new ApiResponse<StaffDashboardResponseDto> { Success = true, Data = result });
        }
    }
}
