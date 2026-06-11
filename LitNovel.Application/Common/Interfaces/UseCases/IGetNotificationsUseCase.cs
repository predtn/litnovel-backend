using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Notification;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetNotificationsUseCase
    {
        Task<PagedResult<NotificationResponseDto>> ExecuteAsync(NotificationQueryDto query, CancellationToken ct);
    }
}
