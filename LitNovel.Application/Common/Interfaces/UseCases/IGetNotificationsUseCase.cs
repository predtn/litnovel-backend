using LitNovel.Application.DTOs.Notification;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetNotificationsUseCase
    {
        Task<NotificationListResponseDto> ExecuteAsync(NotificationQueryDto query, CancellationToken ct);
    }
}
