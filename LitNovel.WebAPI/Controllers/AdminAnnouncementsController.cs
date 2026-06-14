using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;
using LitNovel.WebAPI.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LitNovel.WebAPI.Controllers
{
    [ApiController]
    [Route("api/admin/announcements")]
    [Authorize(Roles = "Admin")]
    public class AdminAnnouncementsController : ControllerBase
    {
        private readonly IGetAdminAnnouncementsUseCase _getAnnouncementsUseCase;
        private readonly ICreateAdminAnnouncementUseCase _createAnnouncementUseCase;
        private readonly IUpdateAdminAnnouncementUseCase _updateAnnouncementUseCase;
        private readonly IDeleteAdminAnnouncementUseCase _deleteAnnouncementUseCase;
        private readonly IToggleAdminAnnouncementUseCase _toggleAnnouncementUseCase;

        public AdminAnnouncementsController(
            IGetAdminAnnouncementsUseCase getAnnouncementsUseCase,
            ICreateAdminAnnouncementUseCase createAnnouncementUseCase,
            IUpdateAdminAnnouncementUseCase updateAnnouncementUseCase,
            IDeleteAdminAnnouncementUseCase deleteAnnouncementUseCase,
            IToggleAdminAnnouncementUseCase toggleAnnouncementUseCase)
        {
            _getAnnouncementsUseCase = getAnnouncementsUseCase;
            _createAnnouncementUseCase = createAnnouncementUseCase;
            _updateAnnouncementUseCase = updateAnnouncementUseCase;
            _deleteAnnouncementUseCase = deleteAnnouncementUseCase;
            _toggleAnnouncementUseCase = toggleAnnouncementUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetAnnouncements(CancellationToken ct)
        {
            var result = await _getAnnouncementsUseCase.ExecuteAsync(ct);
            return Ok(new ApiResponse<IReadOnlyList<AdminAnnouncementResponseDto>> { Success = true, Data = result });
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnnouncement(CreateAdminAnnouncementRequestDto request, CancellationToken ct)
        {
            var result = await _createAnnouncementUseCase.ExecuteAsync(request, ct);
            return StatusCode(StatusCodes.Status201Created, new ApiResponse<AdminAnnouncementSummaryResponseDto>
            {
                Success = true,
                Message = "Announcement created",
                Data = result
            });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAnnouncement(int id, UpdateAdminAnnouncementRequestDto request, CancellationToken ct)
        {
            var result = await _updateAnnouncementUseCase.ExecuteAsync(id, request, ct);
            return Ok(new ApiResponse<AdminAnnouncementResponseDto>
            {
                Success = true,
                Message = "Announcement updated",
                Data = result
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAnnouncement(int id, CancellationToken ct)
        {
            await _deleteAnnouncementUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<object> { Success = true, Data = null });
        }

        [HttpPut("{id:int}/toggle")]
        public async Task<IActionResult> ToggleAnnouncement(int id, CancellationToken ct)
        {
            var result = await _toggleAnnouncementUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<AdminAnnouncementResponseDto>
            {
                Success = true,
                Message = result.IsActive ? "Announcement activated" : "Announcement deactivated",
                Data = result
            });
        }
    }
}
