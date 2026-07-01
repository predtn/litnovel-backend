using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class DeleteNovelUseCase : IDeleteNovelUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPendingDeletionSettingsProvider _pendingDeletionSettingsProvider;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteNovelUseCase(
            INovelRepository novelRepository,
            ICurrentUserService currentUserService,
            IPendingDeletionSettingsProvider pendingDeletionSettingsProvider,
            IUnitOfWork unitOfWork)
        {
            _novelRepository = novelRepository;
            _currentUserService = currentUserService;
            _pendingDeletionSettingsProvider = pendingDeletionSettingsProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid novel id");
            }

            var novel = await _novelRepository.GetByIdForDeleteAsync(id, ct);
            if (novel == null)
            {
                throw new NotFoundException("Novel not found");
            }

            if (!CanManage(novel.AuthorId))
            {
                throw new ForbiddenException("You do not have permission to delete this novel");
            }

            if (novel.Status == NovelStatus.Draft)
            {
                _novelRepository.Delete(novel);
                await _unitOfWork.SaveChangesAsync(ct);
                return;
            }

            if (!IsApprovedStatus(novel.Status))
            {
                throw new BadRequestException("Only draft novels can be deleted directly. Approved novels can only be scheduled for deletion.");
            }

            var now = DateTime.UtcNow;
            novel.PreviousPublicStatus = novel.Status;
            novel.Status = NovelStatus.PendingDeletion;
            novel.DeletionRequestedAt = now;
            novel.ScheduledHardDeleteAt = now.AddSeconds(_pendingDeletionSettingsProvider.GetSettings().RetentionSeconds);
            novel.DeletionRequestedById = _currentUserService.UserId;
            await _unitOfWork.SaveChangesAsync(ct);
        }

        private bool CanManage(int authorId)
        {
            return authorId == _currentUserService.UserId
                || string.Equals(_currentUserService.Role, UserRole.Staff.ToString(), StringComparison.OrdinalIgnoreCase)
                || string.Equals(_currentUserService.Role, UserRole.Admin.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsApprovedStatus(NovelStatus status)
        {
            return status is NovelStatus.Ongoing or NovelStatus.Ended or NovelStatus.Hiatus or NovelStatus.Dropped or NovelStatus.Canceled;
        }
    }
}
