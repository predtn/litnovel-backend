using LitNovel.Application.Common.Interfaces.UseCases;
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
        private readonly IGetNotificationByIdUseCase _getByIdUseCase;
        private readonly IMarkNotificationReadUseCase _markReadUseCase;
        private readonly IMarkAllNotificationsReadUseCase _markAllReadUseCase;
        private readonly IDeleteNotificationUseCase _deleteUseCase;

        public NotificationsController(
            IGetNotificationsUseCase getNotificationsUseCase,
            IGetNotificationByIdUseCase getByIdUseCase,
            IMarkNotificationReadUseCase markReadUseCase,
            IMarkAllNotificationsReadUseCase markAllReadUseCase,
            IDeleteNotificationUseCase deleteUseCase)
        {
            _getNotificationsUseCase = getNotificationsUseCase;
            _getByIdUseCase = getByIdUseCase;
            _markReadUseCase = markReadUseCase;
            _markAllReadUseCase = markAllReadUseCase;
            _deleteUseCase = deleteUseCase;
        }

        /// <summary>GET /api/notifications?isRead=false&amp;type=NewComment&amp;page=1&amp;size=20</summary>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] NotificationQueryDto query, CancellationToken ct)
        {
            var result = await _getNotificationsUseCase.ExecuteAsync(query, ct);
            return Ok(new ApiResponse<NotificationListResponseDto> { Success = true, Data = result });
        }

        /// <summary>GET /api/notifications/{id}</summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await _getByIdUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<NotificationResponseDto> { Success = true, Data = result });
        }

        /// <summary>PUT /api/notifications/{id}/read</summary>
        [HttpPut("{id:int}/read")]
        public async Task<IActionResult> MarkRead(int id, CancellationToken ct)
        {
            await _markReadUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<object> { Success = true, Message = "Notification marked as read." });
        }

        /// <summary>PUT /api/notifications/read-all</summary>
        [HttpPut("read-all")]
        public async Task<IActionResult> MarkAllRead(CancellationToken ct)
        {
            await _markAllReadUseCase.ExecuteAsync(ct);
            return Ok(new ApiResponse<object> { Success = true, Message = "All notifications marked as read." });
        }

        /// <summary>DELETE /api/notifications/{id}</summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await _deleteUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<object> { Success = true, Message = "Notification deleted." });
        }
    }
}
