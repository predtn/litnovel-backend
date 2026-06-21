using LitNovel.Application.DTOs.Notification;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetNotificationByIdUseCase
    {
        Task<NotificationResponseDto> ExecuteAsync(int notificationId, CancellationToken ct);
    }
}
