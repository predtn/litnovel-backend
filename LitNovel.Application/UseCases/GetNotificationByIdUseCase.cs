using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Notification;

namespace LitNovel.Application.UseCases
{
    public class GetNotificationByIdUseCase : IGetNotificationByIdUseCase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetNotificationByIdUseCase(
            INotificationRepository notificationRepository,
            ICurrentUserService currentUserService)
        {
            _notificationRepository = notificationRepository;
            _currentUserService = currentUserService;
        }

        public async Task<NotificationResponseDto> ExecuteAsync(int notificationId, CancellationToken ct)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId, _currentUserService.UserId, ct);
            if (notification == null)
                throw new NotFoundException("Notification not found.");

            return notification;
        }
    }
}
