using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.UseCases
{
    public class AddFavoriteUseCase : IAddFavoriteUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public AddFavoriteUseCase(INovelRepository novelRepository, IFavoriteRepository favoriteRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _novelRepository = novelRepository;
            _favoriteRepository = favoriteRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task ExecuteAsync(int novelId, CancellationToken ct)
        {
            _ = await _novelRepository.GetByIdWithDetailsAsync(novelId, ct)
                ?? throw new NotFoundException("Novel not found");

            if (await _favoriteRepository.GetAsync(_currentUserService.UserId, novelId, ct) is not null)
            {
                throw new ConflictException("Novel already in favorites");
            }

            await _favoriteRepository.AddAsync(new Favorite { UserId = _currentUserService.UserId, NovelId = novelId }, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
