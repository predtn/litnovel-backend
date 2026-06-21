using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;

namespace LitNovel.Application.UseCases
{
    public class MarkAllNotificationsReadUseCase : IMarkAllNotificationsReadUseCase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public MarkAllNotificationsReadUseCase(
            INotificationRepository notificationRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(CancellationToken ct)
        {
            await _notificationRepository.MarkAllAsReadAsync(_currentUserService.UserId, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
