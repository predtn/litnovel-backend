using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Notification;
using LitNovel.Domain.Entities;
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

        public async Task AddAsync(Notification notification, CancellationToken ct)
        {
            await _context.Notifications.AddAsync(notification, ct);
        }
    }
}
