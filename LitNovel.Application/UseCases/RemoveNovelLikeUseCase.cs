using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;

namespace LitNovel.Application.UseCases
{
    public class RemoveNovelLikeUseCase : IRemoveNovelLikeUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly INovelLikeRepository _novelLikeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public RemoveNovelLikeUseCase(INovelRepository novelRepository, INovelLikeRepository novelLikeRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _novelRepository = novelRepository;
            _novelLikeRepository = novelLikeRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task ExecuteAsync(int novelId, CancellationToken ct)
        {
            var novel = await _novelRepository.GetByIdForUpdateAsync(novelId, ct)
                ?? throw new NotFoundException("Novel not found");

            var like = await _novelLikeRepository.GetAsync(_currentUserService.UserId, novelId, ct)
                ?? throw new NotFoundException("Novel like not found");

            _novelLikeRepository.Delete(like);
            novel.LikeCount = Math.Max(0, novel.LikeCount - 1);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
