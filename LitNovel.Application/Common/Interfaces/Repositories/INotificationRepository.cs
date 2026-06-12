using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Notification;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task<PagedResult<NotificationResponseDto>> GetByUserAsync(int userId, NotificationQueryDto query, CancellationToken ct);
        Task AddAsync(Notification notification, CancellationToken ct);
    }
}
