using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class DeleteChapterUseCase : IDeleteChapterUseCase
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPendingDeletionSettingsProvider _pendingDeletionSettingsProvider;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteChapterUseCase(
            IChapterRepository chapterRepository,
            ICurrentUserService currentUserService,
            IPendingDeletionSettingsProvider pendingDeletionSettingsProvider,
            IUnitOfWork unitOfWork)
        {
            _chapterRepository = chapterRepository;
            _currentUserService = currentUserService;
            _pendingDeletionSettingsProvider = pendingDeletionSettingsProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid chapter id");
            }

            var chapter = await _chapterRepository.GetByIdForDeleteAsync(id, ct);
            if (chapter == null)
            {
                throw new NotFoundException("Chapter not found");
            }

            if (!VolumePermissionHelper.CanManage(_currentUserService, chapter.Volume.Novel.AuthorId))
            {
                throw new ForbiddenException("You do not have permission to edit this novel");
            }

            if (chapter.Status == ChapterStatus.Draft)
            {
                chapter.Volume.Novel.TotalChapters = Math.Max(0, chapter.Volume.Novel.TotalChapters - 1);
                _chapterRepository.Delete(chapter);
                await _unitOfWork.SaveChangesAsync(ct);
                return;
            }

            if (!IsApprovedStatus(chapter.Status))
            {
                throw new BadRequestException("Only draft chapters can be deleted directly. Approved chapters can only be scheduled for deletion.");
            }

            var now = DateTime.UtcNow;
            chapter.PreviousPublicStatus = chapter.Status;
            chapter.Status = ChapterStatus.PendingDeletion;
            chapter.DeletionRequestedAt = now;
            chapter.ScheduledHardDeleteAt = now.AddSeconds(_pendingDeletionSettingsProvider.GetSettings().RetentionSeconds);
            chapter.DeletionRequestedById = _currentUserService.UserId;
            await _unitOfWork.SaveChangesAsync(ct);
        }

        private static bool IsApprovedStatus(ChapterStatus status)
        {
            return status is ChapterStatus.Published or ChapterStatus.Scheduled;
        }
    }
}
