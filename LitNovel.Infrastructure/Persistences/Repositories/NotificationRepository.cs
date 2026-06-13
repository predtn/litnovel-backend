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

        public async Task<PagedResult<NotificationResponseDto>> GetByUserAsync(int userId, NotificationQueryDto query, CancellationToken ct)
        {
            var page = query.Page <= 0 ? 1 : query.Page;
            var size = query.Size <= 0 ? 20 : query.Size;

            var notifications = _context.Notifications
                .AsNoTracking()
                .Where(n => n.UserId == userId);

            if (query.IsRead.HasValue)
            {
                notifications = notifications.Where(n => n.IsRead == query.IsRead.Value);
            }

            var total = await notifications.CountAsync(ct);
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

            return new PagedResult<NotificationResponseDto>
            {
                Items = items,
                Page = page,
                Size = size,
                TotalElements = total,
                TotalPages = (int)Math.Ceiling(total / (double)size)
            };
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

        public Task AddRangeAsync(IEnumerable<Notification> notifications, CancellationToken ct)
        {
            return _context.Notifications.AddRangeAsync(notifications, ct);
        }
    }
}
