using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Chapter;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class SubmitChapterUseCase : ISubmitChapterUseCase
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public SubmitChapterUseCase(
            IChapterRepository chapterRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _chapterRepository = chapterRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<SubmitChapterResponseDto> ExecuteAsync(int id, CancellationToken ct)
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
                throw new ForbiddenException("You do not have permission to edit this novel");
            }

            if (chapter.Status != ChapterStatus.Draft)
            {
                throw new BadRequestException("Chapter must be in Draft status to submit");
            }

            chapter.Status = ChapterStatus.Pending;
            await _unitOfWork.SaveChangesAsync(ct);

            return new SubmitChapterResponseDto
            {
                Id = chapter.Id,
                Status = chapter.Status.ToString(),
                UpdatedAt = chapter.UpdatedAt
            };
        }
    }
}
