using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;

namespace LitNovel.Application.UseCases
{
    public class DeleteReadingHistoryUseCase : IDeleteReadingHistoryUseCase
    {
        private readonly IReadingProgressRepository _readingProgressRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteReadingHistoryUseCase(IReadingProgressRepository readingProgressRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _readingProgressRepository = readingProgressRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task ExecuteAsync(int novelId, CancellationToken ct)
        {
            var progress = await _readingProgressRepository.GetByUserAndNovelAsync(_currentUserService.UserId, novelId, ct)
                ?? throw new NotFoundException("Reading history not found");

            _readingProgressRepository.Delete(progress);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
