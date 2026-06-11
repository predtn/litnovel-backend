using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Notification;

namespace LitNovel.Application.UseCases
{
    public class GetNotificationsUseCase : IGetNotificationsUseCase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetNotificationsUseCase(INotificationRepository notificationRepository, ICurrentUserService currentUserService)
        {
            _notificationRepository = notificationRepository;
            _currentUserService = currentUserService;
        }

        public Task<PagedResult<NotificationResponseDto>> ExecuteAsync(NotificationQueryDto query, CancellationToken ct)
        {
            return _notificationRepository.GetByUserAsync(_currentUserService.UserId, query, ct);
        }
    }
}
