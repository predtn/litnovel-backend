using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Application.DTOs.Notification;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        public const string AdminNotificationEntityType = "AdminNotification";

        Task<NotificationListResponseDto> GetByUserAsync(int userId, NotificationQueryDto query, CancellationToken ct);
        Task<NotificationResponseDto?> GetByIdAsync(int notificationId, int userId, CancellationToken ct);
        Task AddAsync(Notification notification, CancellationToken ct);
        Task<PagedResult<AdminSentNotificationResponseDto>> GetAdminSentAsync(AdminSentNotificationsQueryDto query, CancellationToken ct);
        IQueryable<AdminSentNotificationResponseDto> QueryAdminSent();
        Task AddRangeAsync(IEnumerable<Notification> notifications, CancellationToken ct);
        Task<bool> MarkAsReadAsync(int notificationId, int userId, CancellationToken ct);
        Task MarkAllAsReadAsync(int userId, CancellationToken ct);
        Task<bool> DeleteAsync(int notificationId, int userId, CancellationToken ct);
    }
}

