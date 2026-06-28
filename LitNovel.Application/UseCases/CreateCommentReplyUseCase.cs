using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Comment;
using LitNovel.Application.DTOs.Notification;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class CreateCommentReplyUseCase : ICreateCommentReplyUseCase
    {
        private readonly ICommentChapterRepository _commentChapterRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationPushService _notificationPush;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IValidator<CreateCommentRequestDto> _validator;

        public CreateCommentReplyUseCase(
            ICommentChapterRepository commentChapterRepository,
            INotificationRepository notificationRepository,
            INotificationPushService notificationPush,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IValidator<CreateCommentRequestDto> validator)
        {
            _commentChapterRepository = commentChapterRepository;
            _notificationRepository = notificationRepository;
            _notificationPush = notificationPush;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _validator = validator;
        }

        public async Task<CommentResponseDto> ExecuteAsync(int commentId, CreateCommentRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var parent = await _commentChapterRepository.GetByIdAsync(commentId, ct)
                ?? throw new NotFoundException("Comment not found");

            var reply = new CommentChapter
            {
                ChapterId = parent.ChapterId,
                UserId = _currentUserService.UserId,
                ParentCommentId = parent.Id,
                Content = request.Content.Trim()
            };

            await _commentChapterRepository.AddAsync(reply, ct);

            // Trigger CommentReply notification to owner of the parent comment (skip if replier is the same person)
            Notification? notification = null;
            if (parent.UserId != _currentUserService.UserId)
            {
                notification = new Notification
                {
                    UserId = parent.UserId,
                    NotificationType = NotificationType.CommentReply,
                    EntityType = "Comment",
                    EntityId = parent.Id,
                    Message = "Có người vừa trả lời bình luận của bạn.",
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
                await _notificationPush.PushAsync(parent.UserId, pushDto, ct);
            }

            return new CommentResponseDto
            {
                Id = reply.Id,
                User = new CommentUserResponseDto { Id = reply.UserId, Username = string.Empty },
                Content = reply.Content,
                LikeCount = reply.LikeCount,
                DislikeCount = reply.DislikeCount,
                ParentCommentId = reply.ParentCommentId,
                CreatedAt = reply.CreatedAt
            };
        }
    }
}
