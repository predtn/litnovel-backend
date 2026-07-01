using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Chapter;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class RestoreChapterUseCase : IRestoreChapterUseCase
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public RestoreChapterUseCase(
            IChapterRepository chapterRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _chapterRepository = chapterRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateChapterResponseDto> ExecuteAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid chapter id");
            }

            var chapter = await _chapterRepository.GetByIdForUpdateAsync(id, ct);
            if (chapter == null)
            {
                throw new NotFoundException("Chapter not found");
            }

            if (!VolumePermissionHelper.CanManage(_currentUserService, chapter.Volume.Novel.AuthorId))
            {
                throw new ForbiddenException("You do not have permission to restore this chapter");
            }

            if (chapter.Status != ChapterStatus.PendingDeletion)
            {
                throw new BadRequestException("Chapter is not pending deletion");
            }

            chapter.Status = chapter.PreviousPublicStatus ?? ChapterStatus.Published;
            chapter.PreviousPublicStatus = null;
            chapter.DeletionRequestedAt = null;
            chapter.ScheduledHardDeleteAt = null;
            chapter.DeletionRequestedById = null;

            await _unitOfWork.SaveChangesAsync(ct);

            return new UpdateChapterResponseDto
            {
                Id = chapter.Id,
                Title = chapter.Title,
                Status = chapter.Status.ToString(),
                UpdatedAt = chapter.UpdatedAt
            };
        }
    }
}
