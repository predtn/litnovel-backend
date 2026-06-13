using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Application.DTOs.Notification;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        public const string AdminNotificationEntityType = "AdminNotification";

        Task<PagedResult<NotificationResponseDto>> GetByUserAsync(int userId, NotificationQueryDto query, CancellationToken ct);
        Task<PagedResult<AdminSentNotificationResponseDto>> GetAdminSentAsync(AdminSentNotificationsQueryDto query, CancellationToken ct);
        Task AddRangeAsync(IEnumerable<Notification> notifications, CancellationToken ct);
    }
}
