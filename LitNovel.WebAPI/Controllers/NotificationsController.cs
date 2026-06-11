using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Notification;
using LitNovel.WebAPI.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LitNovel.WebAPI.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly IGetNotificationsUseCase _getNotificationsUseCase;

        public NotificationsController(IGetNotificationsUseCase getNotificationsUseCase)
        {
            _getNotificationsUseCase = getNotificationsUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] NotificationQueryDto query, CancellationToken ct)
        {
            var result = await _getNotificationsUseCase.ExecuteAsync(query, ct);
            return Ok(new ApiResponse<PagedResult<NotificationResponseDto>> { Success = true, Data = result });
        }
    }
}
