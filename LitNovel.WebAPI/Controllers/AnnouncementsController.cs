using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Announcement;
using LitNovel.WebAPI.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace LitNovel.WebAPI.Controllers
{
    [ApiController]
    [Route("api/announcements")]
    public class AnnouncementsController : ControllerBase
    {
        private readonly IGetAnnouncementsUseCase _getAnnouncementsUseCase;

        public AnnouncementsController(IGetAnnouncementsUseCase getAnnouncementsUseCase)
        {
            _getAnnouncementsUseCase = getAnnouncementsUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken ct)
        {
            var result = await _getAnnouncementsUseCase.ExecuteAsync(ct);
            return Ok(new ApiResponse<IReadOnlyList<AnnouncementResponseDto>> { Success = true, Data = result });
        }
    }
}
