using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;

namespace LitNovel.Application.UseCases
{
    public class DeleteReadingHistoryUseCase : IDeleteReadingHistoryUseCase
    {
        private readonly IReadingProgressRepository _readingProgressRepository;
        private readonly IChapterReadRepository _chapterReadRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteReadingHistoryUseCase(
            IReadingProgressRepository readingProgressRepository,
            IChapterReadRepository chapterReadRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _readingProgressRepository = readingProgressRepository;
            _chapterReadRepository = chapterReadRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task ExecuteAsync(int novelId, CancellationToken ct)
        {
            var progress = await _readingProgressRepository.GetByUserAndNovelAsync(_currentUserService.UserId, novelId, ct)
                ?? throw new NotFoundException("Reading history not found");
            var chapterReads = await _chapterReadRepository.GetByUserAndNovelForDeleteAsync(_currentUserService.UserId, novelId, ct);

            _readingProgressRepository.Delete(progress);
            _chapterReadRepository.DeleteRange(chapterReads);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
