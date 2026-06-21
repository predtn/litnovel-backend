using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;

namespace LitNovel.Application.UseCases
{
    public class MarkNotificationReadUseCase : IMarkNotificationReadUseCase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public MarkNotificationReadUseCase(
            INotificationRepository notificationRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(int notificationId, CancellationToken ct)
        {
            var found = await _notificationRepository.MarkAsReadAsync(notificationId, _currentUserService.UserId, ct);
            if (!found)
                throw new NotFoundException("Notification not found.");

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
