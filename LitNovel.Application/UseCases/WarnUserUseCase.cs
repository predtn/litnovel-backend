using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Staff;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class WarnUserUseCase : IWarnUserUseCase
    {
        private readonly IUserRepository        _userRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork            _unitOfWork;

        public WarnUserUseCase(
            IUserRepository userRepository,
            INotificationRepository notificationRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository         = userRepository;
            _notificationRepository = notificationRepository;
            _unitOfWork             = unitOfWork;
        }

        public async Task ExecuteAsync(WarnUserRequestDto request, CancellationToken ct)
        {
            if (request.UserId <= 0)
                throw new BadRequestException("UserId is required.");

            if (string.IsNullOrWhiteSpace(request.Message))
                throw new BadRequestException("Warning message is required.");

            var user = await _userRepository.GetByIdAsync(request.UserId, ct)
                ?? throw new NotFoundException("User not found.");

            var notification = new Notification
            {
                UserId           = user.Id,
                NotificationType = NotificationType.SystemAlert,
                EntityType       = "Warning",
                EntityId         = null,
                Message          = $"⚠️ Cảnh báo từ Ban kiểm duyệt: {request.Message}",
                IsRead           = false
            };

            await _notificationRepository.AddAsync(notification, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
