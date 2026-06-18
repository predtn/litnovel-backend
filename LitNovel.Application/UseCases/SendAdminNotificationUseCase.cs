using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class SendAdminNotificationUseCase : ISendAdminNotificationUseCase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<SendAdminNotificationRequestDto> _validator;

        public SendAdminNotificationUseCase(
            INotificationRepository notificationRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IValidator<SendAdminNotificationRequestDto> validator)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<SendAdminNotificationResponseDto> ExecuteAsync(SendAdminNotificationRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var notificationType = Enum.Parse<NotificationType>(request.NotificationType, true);
            var message = request.Message.Trim();
            var recipientIds = await GetRecipientIdsAsync(request, ct);

            var notifications = recipientIds
                .Select(userId => new Notification
                {
                    UserId = userId,
                    NotificationType = notificationType,
                    EntityType = INotificationRepository.AdminNotificationEntityType,
                    Message = message,
                    IsRead = false
                })
                .ToList();

            await _notificationRepository.AddRangeAsync(notifications, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return new SendAdminNotificationResponseDto
            {
                SentCount = notifications.Count,
                SentAt = notifications.Count == 0 ? DateTime.UtcNow : notifications[0].CreatedAt
            };
        }

        private async Task<IReadOnlyList<int>> GetRecipientIdsAsync(SendAdminNotificationRequestDto request, CancellationToken ct)
        {
            if (request.TargetAll == true)
            {
                return await _userRepository.GetAllIdsAsync(ct);
            }

            var user = await _userRepository.GetByIdAsync(request.TargetUserId!.Value, ct)
                ?? throw new NotFoundException("Target user not found");

            return new List<int> { user.Id };
        }
    }
}
