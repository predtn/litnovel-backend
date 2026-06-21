using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;

namespace LitNovel.Application.UseCases
{
    public class DeleteNotificationUseCase : IDeleteNotificationUseCase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteNotificationUseCase(
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
            var deleted = await _notificationRepository.DeleteAsync(notificationId, _currentUserService.UserId, ct);
            if (!deleted)
                throw new NotFoundException("Notification not found.");

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
