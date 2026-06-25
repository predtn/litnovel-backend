using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.DTOs.Notification;
using LitNovel.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace LitNovel.Infrastructure.Services
{
    public class SignalRNotificationPushService : INotificationPushService
    {
        private readonly IHubContext<NotificationHub> _hub;

        public SignalRNotificationPushService(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public Task PushAsync(int userId, NotificationResponseDto notification, CancellationToken ct)
            => _hub.Clients
                .Group($"user-{userId}")
                .SendAsync("ReceiveNotification", notification, ct);
    }
}
