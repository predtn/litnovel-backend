using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Novel;

namespace LitNovel.Application.UseCases
{
    public class GetMyNovelLikesUseCase : IGetMyNovelLikesUseCase
    {
        private readonly INovelLikeRepository _novelLikeRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetMyNovelLikesUseCase(INovelLikeRepository novelLikeRepository, ICurrentUserService currentUserService)
        {
            _novelLikeRepository = novelLikeRepository;
            _currentUserService = currentUserService;
        }

        public Task<PagedResult<NovelListItemResponseDto>> ExecuteAsync(int page, int size, CancellationToken ct)
        {
            return _novelLikeRepository.GetByUserAsync(_currentUserService.UserId, page, size, ct);
        }
    }
}
