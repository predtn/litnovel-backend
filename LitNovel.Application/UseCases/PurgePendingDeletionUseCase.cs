using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;

namespace LitNovel.Application.UseCases
{
    public class PurgePendingDeletionUseCase : IPurgePendingDeletionUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly IChapterRepository _chapterRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PurgePendingDeletionUseCase(
            INovelRepository novelRepository,
            IChapterRepository chapterRepository,
            IUnitOfWork unitOfWork)
        {
            _novelRepository = novelRepository;
            _chapterRepository = chapterRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(DateTime utcNow, CancellationToken ct)
        {
            var expiredNovels = await _novelRepository.GetExpiredPendingDeletionAsync(utcNow, ct);
            foreach (var novel in expiredNovels)
            {
                _novelRepository.Delete(novel);
            }

            var expiredChapters = await _chapterRepository.GetExpiredPendingDeletionAsync(utcNow, ct);
            foreach (var chapter in expiredChapters)
            {
                _chapterRepository.Delete(chapter);
            }

            if (expiredNovels.Count > 0 || expiredChapters.Count > 0)
            {
                await _unitOfWork.SaveChangesAsync(ct);
            }
        }
    }
}
