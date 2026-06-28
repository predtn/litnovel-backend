using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Notification;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class AddCommentLikeUseCase : IAddCommentLikeUseCase
    {
        private readonly ICommentChapterRepository _commentChapterRepository;
        private readonly ICommentLikeRepository _commentLikeRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationPushService _notificationPush;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public AddCommentLikeUseCase(
            ICommentChapterRepository commentChapterRepository,
            ICommentLikeRepository commentLikeRepository,
            INotificationRepository notificationRepository,
            INotificationPushService notificationPush,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _commentChapterRepository = commentChapterRepository;
            _commentLikeRepository = commentLikeRepository;
            _notificationRepository = notificationRepository;
            _notificationPush = notificationPush;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task ExecuteAsync(int commentId, CancellationToken ct)
        {
            var comment = await _commentChapterRepository.GetByIdAsync(commentId, ct)
                ?? throw new NotFoundException("Comment not found");

            if (await _commentLikeRepository.GetAsync(_currentUserService.UserId, commentId, ct) is not null)
            {
                throw new ConflictException("Comment already liked");
            }

            await _commentLikeRepository.AddAsync(new CommentLike { UserId = _currentUserService.UserId, CommentChapterId = commentId }, ct);
            comment.LikeCount++;

            // Trigger CommentLike notification to the comment owner (skip if liker is the owner)
            Notification? notification = null;
            if (comment.UserId != _currentUserService.UserId)
            {
                notification = new Notification
                {
                    UserId = comment.UserId,
                    NotificationType = NotificationType.CommentLike,
                    EntityType = "Comment",
                    EntityId = commentId,
                    Message = "Bình luận của bạn vừa nhận được một lượt thích.",
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
                await _notificationPush.PushAsync(comment.UserId, pushDto, ct);
            }
        }
    }
}
