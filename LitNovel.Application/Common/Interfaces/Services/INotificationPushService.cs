using LitNovel.Application.DTOs.Notification;

namespace LitNovel.Application.Common.Interfaces.Services
{
    /// <summary>
    /// Pushes a real-time notification to a connected user via SignalR.
    /// </summary>
    public interface INotificationPushService
    {
        Task PushAsync(int userId, NotificationResponseDto notification, CancellationToken ct);
    }
}
