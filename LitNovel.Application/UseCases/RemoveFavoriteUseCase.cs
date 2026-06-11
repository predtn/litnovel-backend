using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;

namespace LitNovel.Application.UseCases
{
    public class RemoveFavoriteUseCase : IRemoveFavoriteUseCase
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public RemoveFavoriteUseCase(IFavoriteRepository favoriteRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _favoriteRepository = favoriteRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task ExecuteAsync(int novelId, CancellationToken ct)
        {
            var favorite = await _favoriteRepository.GetAsync(_currentUserService.UserId, novelId, ct)
                ?? throw new NotFoundException("Favorite not found");

            _favoriteRepository.Delete(favorite);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
