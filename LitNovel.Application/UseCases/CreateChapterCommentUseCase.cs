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
    public class CreateChapterCommentUseCase : ICreateChapterCommentUseCase
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly ICommentChapterRepository _commentChapterRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationPushService _notificationPush;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IValidator<CreateCommentRequestDto> _validator;

        public CreateChapterCommentUseCase(
            IChapterRepository chapterRepository,
            ICommentChapterRepository commentChapterRepository,
            INotificationRepository notificationRepository,
            INotificationPushService notificationPush,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IValidator<CreateCommentRequestDto> validator)
        {
            _chapterRepository = chapterRepository;
            _commentChapterRepository = commentChapterRepository;
            _notificationRepository = notificationRepository;
            _notificationPush = notificationPush;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _validator = validator;
        }

        public async Task<CommentResponseDto> ExecuteAsync(int chapterId, CreateCommentRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var chapter = await _chapterRepository.GetByIdWithDetailsAsync(chapterId, ct)
                ?? throw new NotFoundException("Chapter not found");

            var comment = new CommentChapter
            {
                ChapterId = chapterId,
                UserId = _currentUserService.UserId,
                Content = request.Content.Trim()
            };

            await _commentChapterRepository.AddAsync(comment, ct);

            // Trigger NewComment notification to the novel's author (skip if commenter is the author)
            int authorId = chapter.Volume.Novel.AuthorId;
            Notification? notification = null;
            if (authorId != _currentUserService.UserId)
            {
                notification = new Notification
                {
                    UserId = authorId,
                    NotificationType = NotificationType.NewComment,
                    EntityType = "Chapter",
                    EntityId = chapterId,
                    Message = $"Có bình luận mới trong chương \"{chapter.Title}\".",
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

            return MapCreated(comment);
        }

        private static CommentResponseDto MapCreated(CommentChapter comment)
        {
            return new CommentResponseDto
            {
                Id = comment.Id,
                User = new CommentUserResponseDto { Id = comment.UserId, Username = string.Empty },
                Content = comment.Content,
                LikeCount = comment.LikeCount,
                DislikeCount = comment.DislikeCount,
                ParentCommentId = comment.ParentCommentId,
                CreatedAt = comment.CreatedAt
            };
        }
    }
}
