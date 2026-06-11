using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Review;

namespace LitNovel.Application.UseCases
{
    public class GetNovelReviewsUseCase : IGetNovelReviewsUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly INovelRatingRepository _novelRatingRepository;

        public GetNovelReviewsUseCase(INovelRepository novelRepository, INovelRatingRepository novelRatingRepository)
        {
            _novelRepository = novelRepository;
            _novelRatingRepository = novelRatingRepository;
        }

        public async Task<PagedResult<NovelReviewResponseDto>> ExecuteAsync(int novelId, int page, int size, CancellationToken ct)
        {
            _ = await _novelRepository.GetByIdWithDetailsAsync(novelId, ct)
                ?? throw new NotFoundException("Novel not found");

            return await _novelRatingRepository.GetByNovelAsync(novelId, page, size, ct);
        }
    }
}
