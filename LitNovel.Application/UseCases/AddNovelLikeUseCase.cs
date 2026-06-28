using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Notification;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class AddNovelLikeUseCase : IAddNovelLikeUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly INovelLikeRepository _novelLikeRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationPushService _notificationPush;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public AddNovelLikeUseCase(
            INovelRepository novelRepository,
            INovelLikeRepository novelLikeRepository,
            INotificationRepository notificationRepository,
            INotificationPushService notificationPush,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _novelRepository = novelRepository;
            _novelLikeRepository = novelLikeRepository;
            _notificationRepository = notificationRepository;
            _notificationPush = notificationPush;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task ExecuteAsync(int novelId, CancellationToken ct)
        {
            var novel = await _novelRepository.GetByIdForUpdateAsync(novelId, ct)
                ?? throw new NotFoundException("Novel not found");

            if (await _novelLikeRepository.GetAsync(_currentUserService.UserId, novelId, ct) is not null)
            {
                throw new ConflictException("Novel already liked");
            }

            await _novelLikeRepository.AddAsync(new NovelLike { UserId = _currentUserService.UserId, NovelId = novelId }, ct);
            novel.LikeCount++;

            // Trigger NewFollower notification to the novel's author (skip if the user likes their own novel)
            int authorId = novel.AuthorId;
            Notification? notification = null;
            if (authorId != _currentUserService.UserId)
            {
                notification = new Notification
                {
                    UserId = authorId,
                    NotificationType = NotificationType.NewFollower,
                    EntityType = "Novel",
                    EntityId = novelId,
                    Message = $"Có người vừa thích truyện \"{novel.Title}\" của bạn.",
                    IsRead = false
                };
                await _notificationRepository.AddAsync(notification, ct);
            }

            await _unitOfWork.SaveChangesAsync(ct);

            if (notification is not null)
            {
                var pushDto = new NotificationResponseDto
                {
                    Id = notification.Id,
                    NotificationType = notification.NotificationType.ToString(),
                    EntityType = notification.EntityType,
                    EntityId = notification.EntityId,
                    Message = notification.Message,
                    IsRead = false,
                    CreatedAt = notification.CreatedAt
                };
                await _notificationPush.PushAsync(authorId, pushDto, ct);
            }
        }
    }
}
