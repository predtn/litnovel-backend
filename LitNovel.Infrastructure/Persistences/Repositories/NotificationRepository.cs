using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Application.DTOs.Notification;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly LitNovelContext _context;

        public NotificationRepository(LitNovelContext context)
        {
            _context = context;
        }

        public async Task<NotificationListResponseDto> GetByUserAsync(int userId, NotificationQueryDto query, CancellationToken ct)
        {
            var page = query.Page <= 0 ? 1 : query.Page;
            var size = query.Size <= 0 ? 20 : query.Size;

            var notifications = _context.Notifications
                .AsNoTracking()
                .Where(n => n.UserId == userId);

            if (query.IsRead.HasValue)
                notifications = notifications.Where(n => n.IsRead == query.IsRead.Value);

            if (!string.IsNullOrWhiteSpace(query.Type)
                && Enum.TryParse<NotificationType>(query.Type, true, out var notifType))
                notifications = notifications.Where(n => n.NotificationType == notifType);

            var total = await notifications.CountAsync(ct);
            var unreadCount = await _context.Notifications
                .AsNoTracking()
                .CountAsync(n => n.UserId == userId && !n.IsRead, ct);

            var items = await notifications
                .OrderByDescending(n => n.CreatedAt)
                .Skip((page - 1) * size)
                .Take(size)
                .Select(n => new NotificationResponseDto
                {
                    Id = n.Id,
                    NotificationType = n.NotificationType.ToString(),
                    EntityType = n.EntityType,
                    EntityId = n.EntityId,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt
                })
                .ToListAsync(ct);

            return new NotificationListResponseDto
            {
                UnreadCount = unreadCount,
                Items = items,
                Page = page,
                Size = size,
                TotalElements = total,
                TotalPages = (int)Math.Ceiling(total / (double)size)
            };
        }

        public async Task AddAsync(Notification notification, CancellationToken ct)
        {
            await _context.Notifications.AddAsync(notification, ct);
        }

        public async Task<PagedResult<AdminSentNotificationResponseDto>> GetAdminSentAsync(AdminSentNotificationsQueryDto query, CancellationToken ct)
        {
            var page = query.Page <= 0 ? 1 : query.Page;
            var size = query.Size <= 0 ? 20 : query.Size;

            var notifications = _context.Notifications
                .AsNoTracking()
                .Where(n => n.EntityType == INotificationRepository.AdminNotificationEntityType);

            if (!string.IsNullOrWhiteSpace(query.NotificationType)
                && Enum.TryParse<NotificationType>(query.NotificationType, true, out var notificationType))
            {
                notifications = notifications.Where(n => n.NotificationType == notificationType);
            }

            if (query.TargetUserId.HasValue)
            {
                notifications = notifications.Where(n => n.UserId == query.TargetUserId.Value);
            }

            var total = await notifications.CountAsync(ct);
            var items = await notifications
                .OrderByDescending(n => n.CreatedAt)
                .Skip((page - 1) * size)
                .Take(size)
                .Select(n => new AdminSentNotificationResponseDto
                {
                    Id = n.Id,
                    NotificationType = n.NotificationType.ToString(),
                    Message = n.Message,
                    TargetUser = new AdminUserSummaryResponseDto
                    {
                        Id = n.User.Id,
                        Username = n.User.Username
                    },
                    IsRead = n.IsRead,
                    SentAt = n.CreatedAt
                })
                .ToListAsync(ct);

            return new PagedResult<AdminSentNotificationResponseDto>
            {
                Items = items,
                Page = page,
                Size = size,
                TotalElements = total,
                TotalPages = (int)Math.Ceiling(total / (double)size)
            };
        }

        public IQueryable<AdminSentNotificationResponseDto> QueryAdminSent()
        {
            return _context.Notifications
                .AsNoTracking()
                .Where(n => n.EntityType == INotificationRepository.AdminNotificationEntityType)
                .Select(n => new AdminSentNotificationResponseDto
                {
                    Id = n.Id,
                    NotificationType = n.NotificationType.ToString(),
                    Message = n.Message,
                    TargetUser = new AdminUserSummaryResponseDto
                    {
                        Id = n.User.Id,
                        Username = n.User.Username
                    },
                    IsRead = n.IsRead,
                    SentAt = n.CreatedAt
                });
        }

        public Task AddRangeAsync(IEnumerable<Notification> notifications, CancellationToken ct)
        {
            return _context.Notifications.AddRangeAsync(notifications, ct);
        }

        public async Task<bool> MarkAsReadAsync(int notificationId, int userId, CancellationToken ct)
        {
            var affected = await _context.Notifications
                .Where(n => n.Id == notificationId && n.UserId == userId && !n.IsRead)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(n => n.IsRead, true)
                    .SetProperty(n => n.UpdatedAt, DateTime.UtcNow), ct);
            return affected > 0;
        }

        public async Task MarkAllAsReadAsync(int userId, CancellationToken ct)
        {
            await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(n => n.IsRead, true)
                    .SetProperty(n => n.UpdatedAt, DateTime.UtcNow), ct);
        }

        public async Task<NotificationResponseDto?> GetByIdAsync(int notificationId, int userId, CancellationToken ct)
        {
            return await _context.Notifications
                .AsNoTracking()
                .Where(n => n.Id == notificationId && n.UserId == userId)
                .Select(n => new NotificationResponseDto
                {
                    Id = n.Id,
                    NotificationType = n.NotificationType.ToString(),
                    EntityType = n.EntityType,
                    EntityId = n.EntityId,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<bool> DeleteAsync(int notificationId, int userId, CancellationToken ct)
        {
            var affected = await _context.Notifications
                .Where(n => n.Id == notificationId && n.UserId == userId)
                .ExecuteDeleteAsync(ct);
            return affected > 0;
        }
    }
}
