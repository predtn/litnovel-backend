using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Notification;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task<PagedResult<NotificationResponseDto>> GetByUserAsync(int userId, NotificationQueryDto query, CancellationToken ct);
    }
}
