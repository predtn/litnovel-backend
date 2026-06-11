using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.UseCases
{
    public class AddNovelLikeUseCase : IAddNovelLikeUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly INovelLikeRepository _novelLikeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public AddNovelLikeUseCase(INovelRepository novelRepository, INovelLikeRepository novelLikeRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
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

            if (await _novelLikeRepository.GetAsync(_currentUserService.UserId, novelId, ct) is not null)
            {
                throw new ConflictException("Novel already liked");
            }

            await _novelLikeRepository.AddAsync(new NovelLike { UserId = _currentUserService.UserId, NovelId = novelId }, ct);
            novel.LikeCount++;
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
