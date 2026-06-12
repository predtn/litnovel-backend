using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Staff;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class ModerateNovelUseCase : IModerateNovelUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public ModerateNovelUseCase(
            INovelRepository novelRepository,
            INotificationRepository notificationRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _novelRepository         = novelRepository;
            _notificationRepository  = notificationRepository;
            _currentUserService      = currentUserService;
            _unitOfWork              = unitOfWork;
        }

        public async Task ExecuteAsync(int novelId, ModerateNovelRequestDto request, CancellationToken ct)
        {
            var novel = await _novelRepository.GetByIdForUpdateAsync(novelId, ct)
                ?? throw new NotFoundException("Novel not found.");

            var action = request.Action?.Trim().ToLowerInvariant()
                ?? throw new BadRequestException("Action is required.");

            NovelStatus newStatus;
            string notificationMessage;

            switch (action)
            {
                case "approve":
                    newStatus = NovelStatus.Ongoing;
                    notificationMessage = $"Tiểu thuyết \"{novel.Title}\" của bạn đã được phê duyệt và xuất bản.";
                    break;
                case "reject":
                    newStatus = NovelStatus.Draft;
                    notificationMessage = $"Tiểu thuyết \"{novel.Title}\" bị từ chối." +
                                         (string.IsNullOrWhiteSpace(request.Reason) ? "" : $" Lý do: {request.Reason}");
                    break;
                case "lock":
                    newStatus = NovelStatus.Locked;
                    notificationMessage = $"Tiểu thuyết \"{novel.Title}\" đã bị khóa." +
                                         (string.IsNullOrWhiteSpace(request.Reason) ? "" : $" Lý do: {request.Reason}");
                    break;
                default:
                    throw new BadRequestException($"Invalid action '{request.Action}'. Valid: Approve, Reject, Lock.");
            }

            novel.Status = newStatus;

            var notification = new Notification
            {
                UserId           = novel.AuthorId,
                NotificationType = NotificationType.SystemAlert,
                EntityType       = "Novel",
                EntityId         = novel.Id,
                Message          = notificationMessage,
                IsRead           = false
            };

            await _notificationRepository.AddAsync(notification, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
