using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Staff;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class ModerateChapterUseCase : IModerateChapterUseCase
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public ModerateChapterUseCase(
            IChapterRepository chapterRepository,
            INotificationRepository notificationRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _chapterRepository      = chapterRepository;
            _notificationRepository = notificationRepository;
            _currentUserService     = currentUserService;
            _unitOfWork             = unitOfWork;
        }

        public async Task ExecuteAsync(int chapterId, ModerateChapterRequestDto request, CancellationToken ct)
        {
            var chapter = await _chapterRepository.GetByIdForUpdateAsync(chapterId, ct)
                ?? throw new NotFoundException("Chapter not found.");

            var action = request.Action?.Trim().ToLowerInvariant()
                ?? throw new BadRequestException("Action is required.");

            ChapterStatus newStatus;
            string notificationMessage;
            int authorId = chapter.Volume.Novel.AuthorId;

            switch (action)
            {
                case "approve":
                    newStatus = ChapterStatus.Published;
                    notificationMessage = $"Chương \"{chapter.Title}\" của bạn đã được phê duyệt và xuất bản.";
                    break;
                case "reject":
                    newStatus = ChapterStatus.Draft;
                    notificationMessage = $"Chương \"{chapter.Title}\" bị từ chối." +
                                         (string.IsNullOrWhiteSpace(request.Reason) ? "" : $" Lý do: {request.Reason}");
                    break;
                case "lock":
                    newStatus = ChapterStatus.Locked;
                    notificationMessage = $"Chương \"{chapter.Title}\" đã bị khóa." +
                                         (string.IsNullOrWhiteSpace(request.Reason) ? "" : $" Lý do: {request.Reason}");
                    break;
                default:
                    throw new BadRequestException($"Invalid action '{request.Action}'. Valid: Approve, Reject, Lock.");
            }

            chapter.Status = newStatus;

            var notification = new Notification
            {
                UserId           = authorId,
                NotificationType = NotificationType.SystemAlert,
                EntityType       = "Chapter",
                EntityId         = chapter.Id,
                Message          = notificationMessage,
                IsRead           = false
            };

            await _notificationRepository.AddAsync(notification, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
