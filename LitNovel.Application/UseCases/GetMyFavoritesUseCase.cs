using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Novel;

namespace LitNovel.Application.UseCases
{
    public class GetMyFavoritesUseCase : IGetMyFavoritesUseCase
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetMyFavoritesUseCase(IFavoriteRepository favoriteRepository, ICurrentUserService currentUserService)
        {
            _favoriteRepository = favoriteRepository;
            _currentUserService = currentUserService;
        }

        public Task<PagedResult<NovelListItemResponseDto>> ExecuteAsync(int page, int size, CancellationToken ct)
        {
            return _favoriteRepository.GetByUserAsync(_currentUserService.UserId, page, size, ct);
        }
    }
}
