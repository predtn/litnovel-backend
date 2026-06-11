using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;

namespace LitNovel.Application.UseCases
{
    public class DeleteChapterUseCase : IDeleteChapterUseCase
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteChapterUseCase(
            IChapterRepository chapterRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _chapterRepository = chapterRepository;
            _currentUserService = currentUserService;
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

            chapter.Volume.Novel.TotalChapters = Math.Max(0, chapter.Volume.Novel.TotalChapters - 1);
            _chapterRepository.Delete(chapter);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
